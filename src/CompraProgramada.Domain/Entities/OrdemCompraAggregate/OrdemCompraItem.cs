using CompraProgramada.Domain.Enums;
using CompraProgramada.Domain.Exceptions;

namespace CompraProgramada.Domain.Entities.OrdemCompraAggregate
{
    /// <summary>
    /// Item individual de uma ordem de compra.
    /// Distingue entre lote padrão (múltiplos de 100, ticker normal)
    /// e mercado fracionário (1-99, ticker com sufixo F).
    /// </summary>
    public class OrdemCompraItem : Entity
    {
        public int OrdemCompraId { get; private set; }
        public OrdemCompra? OrdemCompra { get; private set; }

        public int AcaoId { get; private set; }
        public Acao? Acao { get; private set; }

        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public decimal ValorTotal { get; private set; }
        public TipoMercado TipoMercado { get; private set; }

        private OrdemCompraItem() { }

        public OrdemCompraItem(int acaoId, int quantidade,
                               decimal precoUnitario, decimal valorTotal, TipoMercado tipoMercado)
        {
            if (quantidade <= 0)
                throw new DomainException("Quantidade deve ser maior que zero.");

            AcaoId = acaoId;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
            ValorTotal = valorTotal;
            TipoMercado = tipoMercado;
        }
    }
}
