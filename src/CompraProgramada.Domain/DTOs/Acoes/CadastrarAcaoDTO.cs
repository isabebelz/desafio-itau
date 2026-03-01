namespace CompraProgramada.Domain.DTOs.Acoes
{
    public class CadastrarAcaoDTO
    {
        public required string Codigo { get; set; }
        public required string NomeEmpresa { get; set; }
        public required decimal Preco { get; set; }
    }
}
