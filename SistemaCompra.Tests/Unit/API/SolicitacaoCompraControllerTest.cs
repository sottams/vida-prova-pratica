using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SistemaCompra.API.SolicitacaoCompra;
using SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Unit.API
{
    public class SolicitacaoCompraControllerTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SolicitacaoCompraController _controller;
        public SolicitacaoCompraControllerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new SolicitacaoCompraController(_mediatorMock.Object);
        }

        private const string UsuarioSolicitantePadrao = "Usuario Teste";
        private const string NomeFornecedorPadrao = "Fornecedor Teste";
        private const int QuantidadePadrao = 10;

        [Fact]
        public async Task RegistrarCompra_Sucesso_Retorna201()
        {
            //ARRANGE
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegistrarCompraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MediatR.Unit.Value);

            var command = new RegistrarCompraCommand
            {
                UsuarioSolicitante = UsuarioSolicitantePadrao,
                NomeFornecedor = NomeFornecedorPadrao,
                Itens = new List<ItemCommand>
                {
                    new ItemCommand
                    {
                        IdProduto = Guid.NewGuid(),
                        Quantidade = QuantidadePadrao,
                    }
                }
            };

            //ACT
            var result = await _controller.RegistrarCompra(command);
            //ASSERT
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
        }
        [Fact]
        public async Task RegistrarCompra_CommandValido_EnviaParaMediator()
        {
            //ARRANGE
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegistrarCompraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MediatR.Unit.Value);

            var command = new RegistrarCompraCommand
            {
                UsuarioSolicitante = UsuarioSolicitantePadrao,
                NomeFornecedor = NomeFornecedorPadrao,
                Itens = new List<ItemCommand>
                {
                    new ItemCommand
                    {
                        IdProduto = Guid.NewGuid(),
                        Quantidade = QuantidadePadrao,
                    }
                }
            };

            //ACT
            await _controller.RegistrarCompra(command);

            //ASSERT
            _mediatorMock.Verify(
                m => m.Send(It.IsAny<RegistrarCompraCommand>(), It.IsAny<CancellationToken>()),
                Times.Once
                );
        }
    }
}
