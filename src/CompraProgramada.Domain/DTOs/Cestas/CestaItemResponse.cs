namespace CompraProgramada.Domain.DTOs.Cestas
{
    public record CestaItemResponse(
        int AcaoId,
        string Codigo,
        string NomeEmpresa,
        decimal Percentual
    );
}