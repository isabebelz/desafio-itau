namespace CompraProgramada.Domain.Entities.ContaMasterAggregate
{
    /// <summary>
    /// Conta da corretora que consolida as compras antes da distribuição.
    /// É uma entidade singleton no sistema (apenas uma conta master).
    /// Mantém a custódia de resíduos entre ciclos de compra.
    /// </summary>
    public class ContaMaster : Entity
    {
        public string Descricao { get; private set; }

        public ICollection<CustodiaMaster> Custodias { get; private set; }

        private ContaMaster()
        {
            Descricao = null!;
            Custodias = [];
        }

        public ContaMaster(string descricao)
        {
            Descricao = descricao;
            Custodias = [];
        }
    }
}
