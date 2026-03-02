namespace CompraProgramada.Domain.DTOs.Acoes
{
    public class ObterAcaoDTO
    {
        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string NomeEmpresa { get; private set; } = string.Empty;
        public decimal Preco { get; private set; }
    }
}
