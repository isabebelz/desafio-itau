namespace CompraProgramada.Domain.DTOs.Acoes
{
    public record ObterAcaoDTO(
        int Id,
        string Codigo,
        string NomeEmpresa,
        decimal Preco
    );
}
