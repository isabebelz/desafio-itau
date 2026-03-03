namespace CompraProgramada.Domain.DTOs.Acoes
{
    public record CadastrarAcaoDTO(
        string Codigo,
        string NomeEmpresa,
        decimal Preco
    );
}
