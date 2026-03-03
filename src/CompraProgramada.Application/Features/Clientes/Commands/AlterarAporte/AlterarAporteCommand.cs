using CompraProgramada.Domain.DTOs.Clientes;
using MediatR;

namespace CompraProgramada.Application.Features.Clientes.Commands.AlterarAporte
{
    public sealed record AlterarAporteCommand(int ClienteId, AlterarAporteDTO Aporte) : IRequest<ClienteResponse>;
}