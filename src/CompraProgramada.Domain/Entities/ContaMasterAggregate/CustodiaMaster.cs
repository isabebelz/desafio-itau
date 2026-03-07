using CompraProgramada.Domain.Exceptions;

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

        public CustodiaMaster(int contaMasterId, int acaoId, int quantidade, decimal precoUnitario)
        {
            if (quantidade <= 0)
                throw new DomainException("Quantidade deve ser maior que zero.");

            ContaMasterId = contaMasterId;
            AcaoId = acaoId;
            Quantidade = quantidade;
            PrecoMedio = precoUnitario;
        }

        /// <summary>
        /// Mesma fórmula de preço médio da custódia filhote.
        /// Atualiza ao receber ativos comprados.
        /// </summary>
        public void AdicionarQuantidade(int quantidade, decimal precoUnitario)
        {
            if (quantidade <= 0)
                throw new DomainException("Quantidade deve ser maior que zero.");

            var valorAnterior = Quantidade * PrecoMedio;
            var valorNovo = quantidade * precoUnitario;

            Quantidade += quantidade;
            PrecoMedio = Math.Round((valorAnterior + valorNovo) / Quantidade, 6);
            DataAtualizacao = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove quantidade ao distribuir para filhotes.
        /// Resíduos permanecem para a próxima compra.
        /// </summary>
        public void RemoverQuantidade(int quantidade)
        {
            if (quantidade <= 0)
                throw new DomainException("Quantidade deve ser maior que zero.");

            if (quantidade > Quantidade)
                throw new DomainException($"Quantidade insuficiente na custódia master. Disponível: {Quantidade}, solicitado: {quantidade}.");

            Quantidade -= quantidade;
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}
