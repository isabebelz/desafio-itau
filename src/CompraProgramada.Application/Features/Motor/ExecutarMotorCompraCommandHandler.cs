using CompraProgramada.Domain.Interfaces.Services;
using MediatR;

namespace CompraProgramada.Application.Features.Motor
{
    public class ExecutarMotorCompraCommandHandler(IMotorCompraService _motorCompraService) : IRequestHandler<ExecutarMotorCompraCommand, string>
    {
        public async Task<string> Handle(ExecutarMotorCompraCommand request, CancellationToken cancellationToken)
        {
            return await _motorCompraService.ExecutarCicloAsync();
        }
    }
}
