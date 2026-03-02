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

        public CustodiaFilhote(int contaGraficaId, int acaoId, int quantidade, decimal precoMedio)
        {
            ContaGraficaId = contaGraficaId;
            AcaoId = acaoId;
            Quantidade = quantidade;
            PrecoMedio = precoMedio;
        }
    }
}
