namespace CompraProgramada.Domain.DTOs.Cestas
{
    public record CadastrarCestaDTO(
        IEnumerable<CadastrarCestaItemDTO> Itens
    );
}