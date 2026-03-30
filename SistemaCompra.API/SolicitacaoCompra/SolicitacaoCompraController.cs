using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaCompra.Application.Produto.Command.AtualizarPreco;
using SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra;
using System.Threading.Tasks;

namespace SistemaCompra.API.SolicitacaoCompra
{
    [ApiController]
    public class SolicitacaoCompraController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SolicitacaoCompraController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost, Route("solicitacao-compra/registrar")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RegistrarCompra([FromBody] RegistrarCompraCommand command)
        {
            await _mediator.Send(command);
            return StatusCode(201);
        }
    }
}
