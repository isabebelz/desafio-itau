using CompraProgramada.Application.Events;
using CompraProgramada.Domain.DTOs.Events;
using CompraProgramada.Domain.Entities.CestaAggregate;
using CompraProgramada.Domain.Entities.ClienteAggregate;
using CompraProgramada.Domain.Entities.ContaMasterAggregate;
using CompraProgramada.Domain.Entities.OrdemCompraAggregate;
using CompraProgramada.Domain.Enums;
using CompraProgramada.Domain.Interfaces;
using CompraProgramada.Domain.Interfaces.Repositories;
using CompraProgramada.Domain.Interfaces.Services;

namespace CompraProgramada.Application.Services;
public class MotorCompraService(IClienteRepository _clienteRepository,
                                  ICestaRecomendacaoRepository _cestaRepository,
                                  IOrdemCompraRepository _ordemRepository,
                                  IAcaoRepository _acaoRepository,
                                  IParametroSistemaRepository _parametroSistemaRepository,
                                  IKafkaProducer _kafkaProducer,
                                  IContaMasterRepository _contaMasterRepository,
                                  ICotacaoRepository _cotacaoRepository) : IMotorCompraService
{
    public async Task<string> ExecutarCicloAsync()
    {
        var clientesAtivos = await BuscarClientesAtivosAsync();
        var cestaAtiva = await BuscarCestaAtivaAsync();
        var aportesPorCliente = CalcularAportesPorCliente(clientesAtivos, out decimal totalAportes);
        var valoresPorAtivo = CalcularValoresPorAtivo(cestaAtiva, totalAportes);

        var cotacoesPorAtivo = await BuscarCotacoesPorAtivoAsync(cestaAtiva);

        var itensOrdem = GerarItensOrdem(valoresPorAtivo, cotacoesPorAtivo);
        var ordemCompra = await CriarOrdemDeCompraAsync(itensOrdem);

        var distribuicoes = await GerarDistribuicoesAsync(
            ordemCompra, clientesAtivos, aportesPorCliente, totalAportes
        );

        await SalvarDistribuicoesAsync(distribuicoes.distribuicoes);
        await AtualizarCustodiasFilhoteAsync(distribuicoes.distribuicoes);
        await AtualizarResiduosNaMasterAsync(ordemCompra, distribuicoes.distribuicoes);
        await PublicarDistribuicoesIrDedoDuroAsync(distribuicoes);

        return $"Ordem consolidada criada: {ordemCompra.Itens.Count} itens, total R$ {ordemCompra.ValorTotal:#,##0.00}.";
    }

    private async Task<IEnumerable<Cliente>> BuscarClientesAtivosAsync()
    {
        var clientes = await _clienteRepository.ObterTodosAtivosAsync();

        if (clientes is null || !clientes.Any())
            throw new InvalidOperationException("Nenhum cliente ativo encontrado para executar o motor de compra.");

        return clientes;
    }

    private async Task<CestaRecomendacao> BuscarCestaAtivaAsync()
    {
        var cesta = await _cestaRepository.ObterAtivaComItensAsync();

        if (cesta is null)
            throw new InvalidOperationException("Não há cesta de recomendação ativa cadastrada.");
        
        return cesta;
    }

    private Dictionary<int, decimal> CalcularAportesPorCliente(IEnumerable<Cliente> clientes, out decimal total)
    {
        var dic = clientes.ToDictionary(
            c => c.Id,
            c => Math.Round(c.ValorAporteMensal / 3m, 2)
        );

        total = dic.Values.Sum();
        return dic;
    }

    private static Dictionary<int, decimal> CalcularValoresPorAtivo(CestaRecomendacao cestaAtiva, decimal totalAportes)
    {
        return cestaAtiva.Itens.ToDictionary(
            item => item.AcaoId,
            item => Math.Round(totalAportes * (item.Percentual / 100m), 2)
        );
    }

    private async Task<Dictionary<int, decimal>> BuscarCotacoesPorAtivoAsync(CestaRecomendacao cestaAtiva)
    {
        var cotacoes = new Dictionary<int, decimal>();
        var acaoIds = cestaAtiva.Itens.Select(i => i.AcaoId).ToList();
        var acoes = await _acaoRepository.ObterPorIdsAsync(acaoIds);

        foreach (var acao in acoes)
        {
            var ultimaCotacao = await _cotacaoRepository.ObterUltimaPorTickerAsync(acao.Codigo);

            if (ultimaCotacao == null)
                throw new InvalidOperationException($"Não foi encontrada nenhuma cotação importada para o ativo {acao.Codigo}. Certifique-se de importar o arquivo da B3 antes de rodar o motor.");

            cotacoes.Add(acao.Id, ultimaCotacao.PrecoFechamento);
        }

        return cotacoes;
    }

    private static List<OrdemCompraItem> GerarItensOrdem(Dictionary<int, decimal> valoresPorAtivo, Dictionary<int, decimal> cotacoesPorAtivo)
    {
        var itens = new List<OrdemCompraItem>();

        foreach (var (acaoId, valorParaAcao) in valoresPorAtivo)
        {
            if (!cotacoesPorAtivo.TryGetValue(acaoId, out decimal cotacao))
                throw new InvalidOperationException($"Cotação não encontrada para AcaoId={acaoId}");

            int quantidadeTotal = (int)Math.Floor(valorParaAcao / cotacao);
            int qtdLote = (quantidadeTotal / 100) * 100;
            int qtdFrac = quantidadeTotal % 100;

            if (qtdLote > 0)
                itens.Add(new OrdemCompraItem(acaoId, qtdLote, cotacao, Math.Round(qtdLote * cotacao, 2), TipoMercado.LotePadrao));
            if (qtdFrac > 0)
                itens.Add(new OrdemCompraItem(acaoId, qtdFrac, cotacao, Math.Round(qtdFrac * cotacao, 2), TipoMercado.Fracionario));
        }

        return itens;
    }

    private async Task<OrdemCompra> CriarOrdemDeCompraAsync(List<OrdemCompraItem> itens)
    {
        var ordemCompra = new OrdemCompra(
            dataExecucao: DateTime.UtcNow,
            valorTotal: itens.Sum(i => i.ValorTotal)
        );

        foreach (var item in itens)
            ordemCompra.Itens.Add(item);

        await _ordemRepository.AdicionarAsync(ordemCompra);

        return ordemCompra;
    }

    private async Task<(List<Distribuicao> distribuicoes, Dictionary<int, int> mapaContaGraficaParaCliente)> GerarDistribuicoesAsync(OrdemCompra ordemCompra, IEnumerable<Cliente> clientesAtivos, Dictionary<int, decimal> aportesPorCliente, decimal totalAportes)
    {
        var distribuicoes = new List<Distribuicao>();
        var mapaContaGraficaParaCliente = new Dictionary<int, int>();

        foreach (var item in ordemCompra.Itens)
        {
            int qtdDisponivel = item.Quantidade;

            foreach (var cliente in clientesAtivos)
            {
                int quantidade = Distribuicao.CalcularQuantidadeDistribuicao(
                    aportecliente: aportesPorCliente[cliente.Id],
                    totalAportes: totalAportes,
                    quantidadeDisponivel: qtdDisponivel
                );

                if (quantidade > 0)
                {
                    var contaGrafica = await _clienteRepository.ObterContaGraficaPorClienteIdAsync(cliente.Id)
                        ?? throw new Exception("Conta gráfica não encontrada para o cliente.");

                    mapaContaGraficaParaCliente[contaGrafica.Id] = cliente.Id;

                    var parametroSistema = await _parametroSistemaRepository.ObterPorChaveAsync("ALIQUOTA_IR_DEDO_DURO");
                    decimal aliquota = parametroSistema != null ? parametroSistema.ObterComoDecimal() : 0.00005m;

                    distribuicoes.Add(new Distribuicao(
                        ordemCompraId: ordemCompra.Id,
                        contaGraficaId: contaGrafica.Id,
                        acaoId: item.AcaoId,
                        quantidade: quantidade,
                        precoUnitario: item.PrecoUnitario,
                        aliquotaIrDedoDuro: aliquota
                    ));
                }
            }
        }

        return (distribuicoes, mapaContaGraficaParaCliente);
    }

    private async Task SalvarDistribuicoesAsync(List<Distribuicao> distribuicoes)
    {
        foreach (var dist in distribuicoes)
            await _ordemRepository.AdicionarDistribuicaoAsync(dist);
    }

    private async Task AtualizarCustodiasFilhoteAsync(List<Distribuicao> distribuicoes)
    {
        foreach (var dist in distribuicoes)
        {
            var custodia = await _clienteRepository.ObterCustodiaPorContaEAcaoAsync(dist.ContaGraficaId, dist.AcaoId);

            if (custodia == null)
            {
                custodia = new CustodiaFilhote(
                    dist.ContaGraficaId, dist.AcaoId, dist.Quantidade, dist.PrecoUnitario);
                await _clienteRepository.AdicionarCustodiaFilhoteAsync(custodia);
            }
            else
            {
                custodia.AdicionarQuantidade(dist.Quantidade, dist.PrecoUnitario);
                await _clienteRepository.AtualizarCustodiaFilhoteAsync(custodia);
            }
        }
    }

    private async Task AtualizarResiduosNaMasterAsync(OrdemCompra ordemCompra, List<Distribuicao> distribuicoes)
    {
        var contaMaster = await _contaMasterRepository.ObterAsync();
        int contaMasterId = contaMaster?.Id ?? 0;

        if (contaMasterId == 0)
            throw new Exception("Conta master não encontrada na base de dados.");

        foreach (var item in ordemCompra.Itens)
        {
            int quantidadeDistribuida = distribuicoes
                .Where(d => d.AcaoId == item.AcaoId)
                .Sum(d => d.Quantidade);

            int residuo = item.Quantidade - quantidadeDistribuida;
            if (residuo > 0)
            {
                var custodiaMaster = await _contaMasterRepository
                .ObterCustodiaPorAcaoAsync(contaMasterId, item.AcaoId);

                if (custodiaMaster == null)
                {
                    custodiaMaster = new CustodiaMaster(
                        contaMasterId, item.AcaoId, residuo, item.PrecoUnitario
                    );

                    await _contaMasterRepository.AdicionarCustodiaAsync(custodiaMaster);
                }
                else
                {
                    custodiaMaster.AdicionarQuantidade(residuo, item.PrecoUnitario);
                    await _contaMasterRepository.AtualizarCustodiaAsync(custodiaMaster);
                }
            }
        }
    }

    private async Task PublicarDistribuicoesIrDedoDuroAsync((List<Distribuicao> distribuicoes, Dictionary<int, int> mapaContaGraficaCliente) tupla)
    {
        var distribuicoes = tupla.distribuicoes;
        var mapaContaGraficaCliente = tupla.mapaContaGraficaCliente;

        var clienteIds = mapaContaGraficaCliente.Values.Distinct().ToList();
        var clientes = (await _clienteRepository.ObterPorIdsAsync(clienteIds))
            .ToDictionary(c => c.Id, c => c);

        var acaoIds = distribuicoes.Select(d => d.AcaoId).Distinct().ToList();
        var acoes = (await _acaoRepository.ObterPorIdsAsync(acaoIds))
            .ToDictionary(a => a.Id, a => a);

        foreach (var dist in distribuicoes)
        {
            var clienteId = mapaContaGraficaCliente[dist.ContaGraficaId];
            var cliente = clientes[clienteId];
            var acao = acoes[dist.AcaoId];

            var aliquotaIrDedoDuro = dist.ValorIRDedoDuro > 0
                ? Math.Round(dist.ValorIRDedoDuro / (dist.Quantidade * dist.PrecoUnitario), 5)
                : 0.00005m;


            var evento = new DistribuicaoIrDedoDuroKafkaEvent
            {
                Tipo = "IR_DEDO_DURO",
                ClienteId = cliente.Id,
                CPF = cliente.CPF,
                Ticker = acao.Codigo,
                TipoOperacao = "COMPRA",
                Quantidade = dist.Quantidade,
                PrecoUnitario = dist.PrecoUnitario,
                ValorOperacao = dist.Quantidade * dist.PrecoUnitario,
                Aliquota = aliquotaIrDedoDuro,
                ValorIR = dist.ValorIRDedoDuro,
                DataOperacao = dist.DataDistribuicao
            };
            await _kafkaProducer.PublicarDistribuicaoIrDedoDuroAsync(evento);
        }
    }
}