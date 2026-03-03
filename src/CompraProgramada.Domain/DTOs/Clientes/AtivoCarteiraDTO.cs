namespace CompraProgramada.Domain.DTOs.Clientes
{
    /// <summary>
    /// Informações de cada ativo na carteira do cliente.
    /// </summary>
    public record AtivoCarteiraDTO(
        string Codigo,
        string NomeEmpresa,
        int Quantidade,
        decimal PrecoMedio,
        decimal CotacaoAtual,
        decimal ValorAtual,
        decimal PL,
        decimal RentabilidadePercentual,
        decimal PercentualCarteira
    );
}