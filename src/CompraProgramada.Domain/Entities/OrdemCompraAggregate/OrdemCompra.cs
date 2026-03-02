using CompraProgramada.Domain.Enums;

namespace CompraProgramada.Domain.Entities.OrdemCompraAggregate
{
    /// <summary>
    /// Registro de compra consolidada executada na conta master.
    /// Agrupa o valor de todos os clientes ativos e registra as ações compradas.
    /// Ciclo: Pendente → Executada → Distribuída.
    /// </summary>
    public class OrdemCompra : Entity
    {
        public DateTime DataExecucao { get; private set; }
        public decimal ValorTotal { get; private set; }
        public StatusOrdem Status { get; private set; }

        public ICollection<OrdemCompraItem> Itens { get; private set; }

        private OrdemCompra()
        {
            Itens = [];
        }

        public OrdemCompra(DateTime dataExecucao, decimal valorTotal, StatusOrdem status)
        {
            DataExecucao = dataExecucao;
            ValorTotal = valorTotal;
            Status = status;
            Itens = [];
        }
    }
}
