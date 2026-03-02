using CompraProgramada.Domain.Exceptions;

namespace CompraProgramada.Domain.Entities.ClienteAggregate
{
    /// <summary>
    /// Posição de ativos de cada cliente, vinculada à sua conta gráfica.
    /// Contém quantidade e preço médio por ativo, atualizados a cada distribuição.
    /// É a base para consulta de carteira, cálculo de P/L e rebalanceamento.
    /// </summary>
    public class CustodiaFilhote : Entity
    {
        public int ContaGraficaId { get; private set; }
        public ContaGrafica? ContaGrafica { get; private set; }

        public int AcaoId { get; private set; }
        public Acao? Acao { get; private set; }

        public int Quantidade { get; private set; }
        public decimal PrecoMedio { get; private set; }

        private CustodiaFilhote() { }

        public CustodiaFilhote(int contaGraficaId, int acaoId, int quantidade, decimal precoUnitario)
        {
            if (quantidade <= 0)
                throw new DomainException("Quantidade deve ser maior que zero.");

            if (precoUnitario <= 0)
                throw new DomainException("Preço unitário deve ser maior que zero.");

            ContaGraficaId = contaGraficaId;
            AcaoId = acaoId;
            Quantidade = quantidade;
            PrecoMedio = precoUnitario;
        }

        /// <summary>
        /// RN-042: Atualiza o preço médio ao receber novas ações (compra).
        /// PM = (Qtd Anterior * PM Anterior + Qtd Nova * Preço Novo) / (Qtd Anterior + Qtd Nova)
        /// </summary>
        public void AdicionarQuantidade(int quantidade, decimal precoUnitario)
        {
            if (quantidade <= 0)
                throw new DomainException("Quantidade deve ser maior que zero.");

            if (precoUnitario <= 0)
                throw new DomainException("Preço unitário deve ser maior que zero.");

            var valorAnterior = Quantidade * PrecoMedio;
            var valorNovo = quantidade * precoUnitario;

            Quantidade += quantidade;
            PrecoMedio = Math.Round((valorAnterior + valorNovo) / Quantidade, 6);
            DataAtualizacao = DateTime.UtcNow;
        }

        /// <summary>
        /// RN-043: Em caso de venda, o preço médio NÃO se altera, apenas a quantidade diminui.
        /// </summary>
        public void RemoverQuantidade(int quantidade)
        {
            if (quantidade <= 0)
                throw new DomainException("Quantidade deve ser maior que zero.");

            if (quantidade > Quantidade)
                throw new DomainException($"Quantidade insuficiente. Disponível: {Quantidade}, solicitado: {quantidade}.");

            Quantidade -= quantidade;
            DataAtualizacao = DateTime.UtcNow;
        }

        /// <summary>
        /// RN-064: Calcula o lucro/prejuízo da posição.
        /// P/L = (Cotação Atual - Preço Médio) * Quantidade
        /// </summary>
        public decimal CalcularPL(decimal cotacaoAtual)
        {
            return Math.Round((cotacaoAtual - PrecoMedio) * Quantidade, 2);
        }

        /// <summary>
        /// RN-066: Calcula a rentabilidade percentual.
        /// Rentabilidade = ((Cotação Atual - Preço Médio) / Preço Médio) * 100
        /// </summary>
        public decimal CalcularRentabilidade(decimal cotacaoAtual)
        {
            if (PrecoMedio == 0) return 0;
            return Math.Round(((cotacaoAtual - PrecoMedio) / PrecoMedio) * 100, 2);
        }

        /// <summary>
        /// RN-063: Calcula o valor atual da posição.
        /// </summary>
        public decimal CalcularValorAtual(decimal cotacaoAtual)
        {
            return Math.Round(Quantidade * cotacaoAtual, 2);
        }

        /// <summary>
        /// Calcula o valor investido (custo de aquisição).
        /// </summary>
        public decimal CalcularValorInvestido()
        {
            return Math.Round(Quantidade * PrecoMedio, 2);
        }

        /// <summary>
        /// RN-060: Calcula o lucro na venda de uma quantidade específica.
        /// Lucro = Quantidade * (Preço Venda - Preço Médio)
        /// </summary>
        public decimal CalcularLucroVenda(int quantidade, decimal precoVenda)
        {
            if (quantidade <= 0 || quantidade > Quantidade)
                throw new DomainException("Quantidade de venda inválida.");

            return Math.Round(quantidade * (precoVenda - PrecoMedio), 2);
        }
    }
}