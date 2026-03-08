using CompraProgramada.Domain.Entities.ClienteAggregate;
using CompraProgramada.Domain.Exceptions;

namespace CompraProgramada.Domain.Entities.OrdemCompraAggregate
{
    /// <summary>
    /// Registro da alocação de ativos da conta master para uma conta filhote.
    /// Cada distribuição gera um cálculo de IR dedo-duro (0,00005% sobre o valor)
    /// que é publicado em um tópico Kafka para a Receita Federal.
    /// </summary>
    public class Distribuicao : Entity
    {

        public int OrdemCompraId { get; private set; }
        public OrdemCompra? OrdemCompra { get; private set; }

        public int ContaGraficaId { get; private set; }
        public ContaGrafica? ContaGrafica { get; private set; }

        public int AcaoId { get; private set; }
        public Acao? Acao { get; private set; }

        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public decimal ValorIRDedoDuro { get; private set; }
        public DateTime DataDistribuicao { get; private set; }

        private Distribuicao() { }

        /// <summary>
        /// Cria uma distribuição calculando automaticamente o IR dedo-duro.
        /// IR = Valor da operação * 0,005%
        /// </summary>
        public Distribuicao(int ordemCompraId, int contaGraficaId, int acaoId,
        {
            if (quantidade <= 0)
                throw new DomainException("Quantidade deve ser maior que zero.");

            if (precoUnitario <= 0)
                throw new DomainException("Preço unitário deve ser maior que zero.");

            OrdemCompraId = ordemCompraId;
            ContaGraficaId = contaGraficaId;
            AcaoId = acaoId;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
            DataDistribuicao = DateTime.UtcNow;
        }

        /// <summary>
        /// Calcula o IR dedo-duro: 0,005% sobre o valor total da operação.
        /// </summary>
        private static decimal CalcularIRDedoDuro(int quantidade, decimal precoUnitario, decimal aliquotaIrDedoDuro)
        {
            var valorOperacao = quantidade * precoUnitario;
            return Math.Round(valorOperacao * aliquotaIrDedoDuro, 2);
        }

        public decimal CalcularValorTotal()
        {
            return Math.Round(Quantidade * PrecoUnitario, 2);
        }

        /// <summary>
        /// Calcula a quantidade a distribuir para um cliente.
        /// Quantidade = TRUNCAR(Proporção * Quantidade Total Disponível)
        /// </summary>
        public static int CalcularQuantidadeDistribuicao(
            decimal aportecliente, decimal totalAportes, int quantidadeDisponivel)
        {
            if (totalAportes <= 0) return 0;

            var proporcao = aportecliente / totalAportes;
            return (int)Math.Truncate(proporcao * quantidadeDisponivel);
        }
    }
}
