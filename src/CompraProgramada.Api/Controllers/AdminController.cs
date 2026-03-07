using CompraProgramada.Application.Features.Cestas.Commands.CadastrarCesta;
using CompraProgramada.Application.Features.Cestas.Queries.ObterCestaAtiva;
using CompraProgramada.Application.Features.Cestas.Queries.ObterHistoricoCestas;
using CompraProgramada.Domain.DTOs.Cestas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompraProgramada.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("cestas")]
        public async Task<IActionResult> CadastrarCesta([FromBody] CadastrarCestaDTO dto)
        {
            var response = await _mediator.Send(new CadastrarCestaCommand(dto));
            return CreatedAtAction(nameof(ObterCestaAtiva), response);
        }


        [HttpGet("cestas/ativa")]
        public async Task<IActionResult> ObterCestaAtiva()
        {
            var response = await _mediator.Send(new ObterCestaAtivaQuery());

            if (response is null)
                return NotFound(new { mensagem = "Nenhuma cesta ativa encontrada." });

            return Ok(response);
        }

        [HttpGet("cestas/historico")]
        public async Task<IActionResult> ObterHistoricoCestas()
        {
            var response = await _mediator.Send(new ObterHistoricoCestasQuery());
            return Ok(response);
        }
    }
}