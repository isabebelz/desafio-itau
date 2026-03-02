using CompraProgramada.Domain.Exceptions;

namespace CompraProgramada.Domain.Entities.CestaAggregate
{
    /// <summary>
    /// Item da cesta de recomendação. Vincula uma ação a um percentual.
    /// Exemplo: PETR4 = 30%, VALE3 = 25%, etc.
    /// A soma de todos os itens de uma cesta deve ser exatamente 100%.
    /// </summary>
    public class CestaRecomendacaoItem : Entity
    {
        public int CestaRecomendacaoId { get; private set; }
        public CestaRecomendacao? CestaRecomendacao { get; private set; }

        public int AcaoId { get; private set; }
        public Acao? Acao { get; private set; }

        public decimal Percentual { get; private set; }

        private CestaRecomendacaoItem() { }

        public CestaRecomendacaoItem(int cestaRecomendacaoId, int acaoId, decimal percentual)
        {
            if (percentual <= 0)
                throw new DomainException("Percentual deve ser maior que 0%.");

            CestaRecomendacaoId = cestaRecomendacaoId;
            AcaoId = acaoId;
            Percentual = percentual;
        }
    }
}
    