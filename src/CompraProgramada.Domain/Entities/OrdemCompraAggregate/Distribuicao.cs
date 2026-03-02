using CompraProgramada.Domain.Entities.ClienteAggregate;

namespace CompraProgramada.Domain.Entities.OrdemCompraAggregate
{
    /// <summary>
    /// Registro da alocação de ativos da conta master para uma conta filhote.
    /// Cada distribuição gera um cálculo de IR dedo-duro (0,005% sobre o valor)
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

        public Distribuicao(int ordemCompraId, int contaGraficaId, int acaoId,
                           int quantidade, decimal precoUnitario, decimal valorIRDedoDuro)
        {
            OrdemCompraId = ordemCompraId;
            ContaGraficaId = contaGraficaId;
            AcaoId = acaoId;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
            ValorIRDedoDuro = valorIRDedoDuro;
            DataDistribuicao = DateTime.UtcNow;
        }
    }
}
