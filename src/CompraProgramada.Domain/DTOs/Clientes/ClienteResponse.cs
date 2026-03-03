namespace CompraProgramada.Domain.DTOs.Clientes
{
    public record ClienteResponse(
        int Id,
        string Nome,
        string CPF,
        string Email,
        decimal ValorAporteMensal,
        bool Ativo,
        DateTime DataAdesao,
        DateTime? DataSaida
    );
}