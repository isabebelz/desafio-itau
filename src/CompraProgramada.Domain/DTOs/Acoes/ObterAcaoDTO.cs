namespace CompraProgramada.Domain.DTOs.Acoes
{
    public class ObterAcaoDTO
    {
        public int Id { get; private set; }
        public string Codigo { get; private set; }
        public string NomeEmpresa { get; private set; }
        public decimal Preco { get; private set; }
    }
}
