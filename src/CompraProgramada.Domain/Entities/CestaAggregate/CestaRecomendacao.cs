namespace CompraProgramada.Domain.Entities.CestaAggregate
{
    /// <summary>
    /// Representa uma versão da carteira recomendada "Top Five".
    /// Contém 5 ações com seus respectivos percentuais (soma = 100%).
    /// Quando o admin altera a cesta, a anterior é desativada e uma nova é criada,
    /// mantendo o histórico de alterações.
    /// </summary>
    public class CestaRecomendacao : Entity
    {
        public bool Ativa { get; private set; }
        public DateTime DataVigencia { get; private set; }

        public ICollection<CestaRecomendacaoItem> Itens { get; private set; }

        private CestaRecomendacao()
        {
            Itens = [];
        }

        public CestaRecomendacao(DateTime dataVigencia)
        {
            Ativa = true;
            DataVigencia = dataVigencia;
            Itens = [];
        }
    }
}
