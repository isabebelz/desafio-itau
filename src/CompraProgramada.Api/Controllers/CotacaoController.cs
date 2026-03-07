using CompraProgramada.Application.Features.Cotacoes.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompraProgramada.Api.Controllers
{
    [ApiController]
    [Route("api/admin/cotacoes")]
    public class CotacoesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CotacoesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("importar")]
        public async Task<IActionResult> Importar([FromBody] string caminhoArquivo)
        {
            var sucesso = await _mediator.Send(new ImportarArquivoCotacaoCommand(caminhoArquivo));

            if (sucesso)
                return Ok("Arquivo processado e cotações importadas.");

            return BadRequest("Falha ao processar o arquivo. Verifique os logs.");
        }
    }
}
