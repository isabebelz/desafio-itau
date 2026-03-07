namespace CompraProgramada.Domain.Entities.CestaAggregate
{
    /// <summary>
    /// Value Object que representa as diferenças entre duas cestas de recomendação.
    /// Usado no processo de rebalanceamento.
    /// </summary>
    public class CestaMudancas
    {
        /// <summary>Ações que entraram na nova cesta.</summary>
        public IReadOnlyList<int> AcoesQueEntraram { get; }

        /// <summary>Ações que saíram da cesta anterior.</summary>
        public IReadOnlyList<int> AcoesQueSairam { get; }

        /// <summary>Ações que permaneceram mas mudaram de percentual.</summary>
        public IReadOnlyList<int> AcoesQueAlteraramPercentual { get; }

        /// <summary>Ações que permaneceram com o mesmo percentual.</summary>
        public IReadOnlyList<int> AcoesQueMantiveram { get; }

        public CestaMudancas(
            IEnumerable<int> entraram,
            IEnumerable<int> sairam,
            IEnumerable<int> alteraram,
            IEnumerable<int> mantiveram)
        {
            AcoesQueEntraram = entraram.ToList().AsReadOnly();
            AcoesQueSairam = sairam.ToList().AsReadOnly();
            AcoesQueAlteraramPercentual = alteraram.ToList().AsReadOnly();
            AcoesQueMantiveram = mantiveram.ToList().AsReadOnly();
        }

        /// <summary>Indica se houve qualquer mudança na composição.</summary>
        public bool HouveMudanca =>
            AcoesQueEntraram.Count > 0 ||
            AcoesQueSairam.Count > 0 ||
            AcoesQueAlteraramPercentual.Count > 0;
    }
}