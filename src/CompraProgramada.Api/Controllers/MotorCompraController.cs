using CompraProgramada.Application.Features.Motor;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace CompraProgramada.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorCompraController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("executar")]
        public async Task<IActionResult> Executar()
        {
            try
            {
                var resultado = await _mediator.Send(new ExecutarMotorCompraCommand());
                return Ok(new { mensagem = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}