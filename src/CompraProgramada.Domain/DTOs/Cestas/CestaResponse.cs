namespace CompraProgramada.Domain.DTOs.Cestas
{
    public record CestaResponse(
        int Id,
        bool Ativa,
        DateTime DataVigencia,
        DateTime DataCriacao,
        IEnumerable<CestaItemResponse> Itens
    );
}