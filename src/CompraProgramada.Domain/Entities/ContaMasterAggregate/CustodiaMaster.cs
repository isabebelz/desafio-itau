namespace CompraProgramada.Domain.Entities.ContaMasterAggregate
{
    /// <summary>
    /// Posição de ativos remanescentes na conta master após a distribuição.
    /// Resíduos de arredondamento ficam aqui e são considerados na próxima compra,
    /// evitando aquisições desnecessárias.
    /// </summary>
    public class CustodiaMaster : Entity
    {
        public int ContaMasterId { get; private set; }
        public ContaMaster? ContaMaster { get; private set; }

        public int AcaoId { get; private set; }
        public Acao? Acao { get; private set; }

        public int Quantidade { get; private set; }
        public decimal PrecoMedio { get; private set; }

        private CustodiaMaster() { }

        public CustodiaMaster(int contaMasterId, int acaoId, int quantidade, decimal precoMedio)
        {
            ContaMasterId = contaMasterId;
            AcaoId = acaoId;
            Quantidade = quantidade;
            PrecoMedio = precoMedio;
        }
    }
}
