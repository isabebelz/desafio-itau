using CompraProgramada.Application.Features.Acoes.Commands.CadastrarAcao;
using CompraProgramada.Application.Features.Acoes.Queries.ObterTodasAcoes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompraProgramada.Api.Controllers
{
    [ApiController]
    [Route("api/acoes")]
    public class AcaoController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CadastrarAcao([FromBody] CadastrarAcaoCommand command)
        {
            int id = await _mediator.Send(command);
            return CreatedAtAction(nameof(CadastrarAcao), new { id }, new { id });
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodasAcoes([FromQuery] bool? ativo)
        {
            var response = await _mediator.Send(new ObterTodasAcoesQuery(ativo));
            return Ok(response);
        }
    }
}
