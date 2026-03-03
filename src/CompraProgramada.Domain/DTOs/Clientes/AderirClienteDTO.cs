namespace CompraProgramada.Domain.DTOs.Clientes
{
    public record AderirClienteDTO(
        string Nome,
        string CPF,
        string Email,
        decimal ValorAporteMensal
    );


}