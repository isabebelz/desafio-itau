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
            if (string.IsNullOrWhiteSpace(descricao))
                throw new DomainException("Descrição é obrigatória.");

            Descricao = descricao;
            Custodias = [];
        }

        /// <summary>
        /// Obtém o saldo de um ativo na custódia master.
        /// Usado para descontar do total a comprar.
        /// </summary>
        public int ObterSaldoAtivo(int acaoId)
        {
            var custodia = Custodias.FirstOrDefault(c => c.AcaoId == acaoId);
            return custodia?.Quantidade ?? 0;
        }
    }
}
