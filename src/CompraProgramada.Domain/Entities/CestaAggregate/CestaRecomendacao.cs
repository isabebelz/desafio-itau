using CompraProgramada.Domain.Exceptions;

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
        public const int TOTAL_ITENS = 5;
        public const decimal PERCENTUAL_TOTAL = 100.00m;

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

        /// <summary>
        /// Adiciona um item à cesta validando limite de 5 e percentual > 0.
        /// </summary>

        public void AdicionarItem(int acaoId, decimal percentual)
        {
            if (Itens.Count >= TOTAL_ITENS)
                throw new DomainException($"A cesta deve conter exatamente {TOTAL_ITENS} ações.");

            if (percentual <= 0)
                throw new DomainException("Percentual deve ser maior que 0%.");

            if (Itens.Any(i => i.AcaoId == acaoId))
                throw new DomainException("Ação já está na cesta.");

            Itens.Add(new CestaRecomendacaoItem(Id, acaoId, percentual));
        }

        /// <summary>
        /// Valida todas as regras da cesta.
        /// - Exatamente 5 ações
        /// - Soma = 100%
        /// - Todos os percentuais > 0
        /// </summary>
        public void Validar()
        {
            if (Itens.Count != TOTAL_ITENS)
                throw new DomainException($"A cesta deve conter exatamente {TOTAL_ITENS} ações. Atual: {Itens.Count}.");

            var soma = Itens.Sum(i => i.Percentual);
            if (soma != PERCENTUAL_TOTAL)
                throw new DomainException($"A soma dos percentuais deve ser {PERCENTUAL_TOTAL}%. Atual: {soma}%.");

            if (Itens.Any(i => i.Percentual <= 0))
                throw new DomainException("Todos os percentuais devem ser maiores que 0%.");
        }

        // <summary>
        /// Desativa a cesta atual ao ser substituída por uma nova.
        /// </summary>
        public void Desativar()
        {
            if (!Ativa)
                throw new DomainException("Cesta já está desativada.");

            Ativa = false;
            DataAtualizacao = DateTime.UtcNow;
        }

        /// <summary>
        /// Identifica as mudanças entre esta cesta e a anterior.
        /// Retorna os IDs dos ativos que entraram, saíram e os que mudaram de percentual.
        /// </summary>
        public CestaMudancas IdentificarMudancas(CestaRecomendacao cestaAnterior)
        {
            var acoeAnterior = cestaAnterior.Itens.ToDictionary(i => i.AcaoId, i => i.Percentual);
            var acoesNova = Itens.ToDictionary(i => i.AcaoId, i => i.Percentual);

            var entraram = acoesNova.Keys.Except(acoeAnterior.Keys).ToList();
            var sairam = acoeAnterior.Keys.Except(acoesNova.Keys).ToList();

            var mudaramPercentual = acoesNova.Keys
                .Intersect(acoeAnterior.Keys)
                .Where(id => acoesNova[id] != acoeAnterior[id])
                .ToList();

            var mantiveram = acoesNova.Keys
                .Intersect(acoeAnterior.Keys)
                .Where(id => acoesNova[id] == acoeAnterior[id])
                .ToList();

            return new CestaMudancas(entraram, sairam, mudaramPercentual, mantiveram);
        }

        /// <summary>
        /// Calcula o valor a comprar de cada ativo dado um valor total.
        /// </summary>
        public Dictionary<int, decimal> CalcularValoresPorAtivo(decimal valorTotal)
        {
            return Itens.ToDictionary(
                i => i.AcaoId,
                i => Math.Round(valorTotal * (i.Percentual / 100m), 2)
            );
        }
    }
}
