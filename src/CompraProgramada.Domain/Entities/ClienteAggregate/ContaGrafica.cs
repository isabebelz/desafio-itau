using CompraProgramada.Domain.Exceptions;

namespace CompraProgramada.Domain.Entities.ClienteAggregate
{
    public class ContaGrafica : Entity
    {
        public int ClienteId { get; private set; }
        public Cliente? Cliente { get; private set; }

        public ICollection<CustodiaFilhote> Custodias { get; private set; }

        private ContaGrafica()
        {
            Custodias = [];
        }

        public ContaGrafica(int clienteId)
        {
            if (clienteId <= 0)
                throw new DomainException("ClienteId inválido.");

            ClienteId = clienteId;
            Custodias = [];
        }

        /// <summary>
        /// Busca a custódia de um ativo específico nesta conta.
        /// </summary>
        public CustodiaFilhote? ObterCustodia(int acaoId)
        {
            return Custodias.FirstOrDefault(c => c.AcaoId == acaoId);
        }

        /// <summary>
        /// Verifica se a conta possui posição em determinado ativo.
        /// </summary>
        public bool PossuiPosicao(int acaoId)
        {
            return Custodias.Any(c => c.AcaoId == acaoId && c.Quantidade > 0);
        }

        /// <summary>
        /// Calcula o valor total da carteira com base nas cotações atuais.
        /// RN-063: Saldo total = soma de (Quantidade * cotação atual) de cada ativo.
        /// </summary>
        public decimal CalcularValorTotal(Func<int, decimal> obterCotacao)
        {
            return Custodias
                .Where(c => c.Quantidade > 0)
                .Sum(c => c.Quantidade * obterCotacao(c.AcaoId));
        }
    }
}