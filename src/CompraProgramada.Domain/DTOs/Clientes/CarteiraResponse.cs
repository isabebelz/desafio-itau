namespace CompraProgramada.Domain.DTOs.Clientes
{
    public record CarteiraResponse(
        int ClienteId,
        string NomeCliente,
        decimal ValorInvestidoTotal,
        decimal ValorAtualTotal,
        decimal PLTotal,
        decimal RentabilidadePercentual,
        IEnumerable<AtivoCarteiraDTO> Ativos
    );
}