using CompraProgramada.Application.Features.Clientes.Commands.AderirCliente;
using CompraProgramada.Application.Features.Clientes.Commands.AlterarAporte;
using CompraProgramada.Application.Features.Clientes.Commands.ReativarCliente;
using CompraProgramada.Application.Features.Clientes.Commands.SairCliente;
using CompraProgramada.Application.Features.Clientes.Queries.ConsultarCarteira;
using CompraProgramada.Domain.DTOs.Clientes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompraProgramada.Api.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Aderir([FromBody] AderirClienteDTO dto)
        {
            var response = await _mediator.Send(new AderirClienteCommand(dto));
            return CreatedAtAction(nameof(Aderir), new { id = response.Id }, response);
        }

        [HttpPut("{id}/sair")]
        public async Task<IActionResult> Sair(int id)
        {
            var response = await _mediator.Send(new SairClienteCommand(id));
            return Ok(response);
        }

        [HttpPut("{id}/reativar")]
        public async Task<IActionResult> Reativar(int id)
        {
            var response = await _mediator.Send(new ReativarClienteCommand(id));
            return Ok(response);
        }

        [HttpPut("{id}/aporte")]
        public async Task<IActionResult> AlterarAporte(int id, [FromBody] AlterarAporteDTO dto)
        {
            var response = await _mediator.Send(new AlterarAporteCommand(id, dto));
            return Ok(response);
        }

        [HttpGet("{id}/carteira")]
        public async Task<IActionResult> ConsultarCarteira(int id)
        {
            var response = await _mediator.Send(new ConsultarCarteiraQuery(id));
            return Ok(response);
        }
    }
}