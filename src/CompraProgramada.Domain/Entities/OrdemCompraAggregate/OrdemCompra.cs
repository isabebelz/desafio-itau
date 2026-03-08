using CompraProgramada.Domain.Enums;
using CompraProgramada.Domain.Exceptions;

namespace CompraProgramada.Domain.Entities.OrdemCompraAggregate
{
    /// <summary>
    /// Registro de compra consolidada executada na conta master.
    /// Agrupa o valor de todos os clientes ativos e registra as ações compradas.
    /// Ciclo: Pendente → Executada → Distribuída.
    /// </summary>
    public class OrdemCompra : Entity
    {
        public const int LOTE_PADRAO = 100;

        public DateTime DataExecucao { get; private set; }
        public decimal ValorTotal { get; private set; }
        public StatusOrdem Status { get; private set; }

        public ICollection<OrdemCompraItem> Itens { get; private set; }

        private OrdemCompra()
        {
            Itens = [];
        }

        public OrdemCompra(DateTime dataExecucao, decimal valorTotal)
        {
            if (valorTotal <= 0)
                throw new DomainException("Valor total da ordem deve ser maior que zero.");

            DataExecucao = dataExecucao;
            ValorTotal = valorTotal;
            Status = StatusOrdem.Pendente;
            Itens = [];
        }

        /// <summary>
        /// Adiciona itens à ordem separando lote padrão e fracionário.
        /// Quantidades >= 100 vão para lote padrão (PETR4), restante para fracionário (PETR4F).
        /// </summary>
        public void AdicionarItensParaAtivo(int acaoId, int quantidadeTotal, decimal precoUnitario)
        {
            if (quantidadeTotal <= 0) return;

            var lotePadrao = quantidadeTotal / LOTE_PADRAO * LOTE_PADRAO;
            var fracionario = quantidadeTotal % LOTE_PADRAO;

            if (lotePadrao > 0)
            {
                Itens.Add(new OrdemCompraItem(
                    acaoId, lotePadrao, precoUnitario,
                    Math.Round(lotePadrao * precoUnitario, 2),
                    TipoMercado.LotePadrao));
            }

            if (fracionario > 0)
            {
                Itens.Add(new OrdemCompraItem(
                    acaoId, fracionario, precoUnitario,
                    Math.Round(fracionario * precoUnitario, 2),
                    TipoMercado.Fracionario));
            }
        }

        /// <summary>
        /// Transição de status: Pendente → Executada.
        /// </summary>
        public void Executar()
        {
            if (Status != StatusOrdem.Pendente)
                throw new DomainException($"Ordem deve estar Pendente para ser executada. Status atual: {Status}.");

            if (Itens.Count == 0)
                throw new DomainException("Ordem sem itens não pode ser executada.");

            Status = StatusOrdem.Executada;
            DataAtualizacao = DateTime.UtcNow;
        }

        /// <summary>
        /// Transição de status: Executada → Distribuída.
        /// </summary>
        public void Distribuir()
        {
            if (Status != StatusOrdem.Executada)
                throw new DomainException($"Ordem deve estar Executada para ser distribuída. Status atual: {Status}.");

            Status = StatusOrdem.Distribuida;
            DataAtualizacao = DateTime.UtcNow;
        }

        /// <summary>
        /// Transição de status: Pendente → Cancelada.
        /// </summary>
        public void Cancelar()
        {
            if (Status != StatusOrdem.Pendente)
                throw new DomainException($"Apenas ordens Pendentes podem ser canceladas. Status atual: {Status}.");

            Status = StatusOrdem.Cancelada;
            DataAtualizacao = DateTime.UtcNow;
        }

        /// <summary>
        ///  Calcula a quantidade a comprar de um ativo.
        /// Quantidade = TRUNCAR(Valor / Cotação) - descontando saldo master.
        /// </summary>
        public static int CalcularQuantidadeAComprar(decimal valorDisponivel, decimal cotacao, int saldoMaster)
        {
            if (cotacao <= 0)
                throw new DomainException("Cotação deve ser maior que zero.");

            var quantidadeCalculada = (int)Math.Truncate(valorDisponivel / cotacao);
            var quantidadeAComprar = quantidadeCalculada - saldoMaster;

            return Math.Max(quantidadeAComprar, 0);
        }
    }
}
