namespace CompraProgramada.Domain.Enums
{
    /// <summary>
    /// LotePadrao: múltiplos de 100 (ticker normal, ex: PETR4)
    /// Fracionario: 1 a 99 unidades (ticker com sufixo F, ex: PETR4F)
    /// </summary>
    public enum TipoMercado
    {
        LotePadrao = 0,
        Fracionario = 1
    }
}
