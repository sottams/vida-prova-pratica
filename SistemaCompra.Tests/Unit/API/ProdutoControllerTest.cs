using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SistemaCompra.API.Produto;
using SistemaCompra.Application.Produto.Command.AtualizarPreco;
using SistemaCompra.Application.Produto.Command.RegistrarProduto;
using SistemaCompra.Application.Produto.Query.ObterProduto;
using SistemaCompra.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Unit.API
{
    public class ProdutoControllerTest
    {

        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProdutoController _controller;

        public ProdutoControllerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProdutoController(_mediatorMock.Object);
        }

        private const string NomeProdutoPadrao = "Produto Teste";
        private const string DescricaoProdutoPadrao = "Descricao Teste";
        private const string CategoriaProdutoPadrao = "Madeira";
        private const decimal PrecoProdutoPadrao = 100;

        //Obter
        [Fact]
        public async Task Obter_Sucesso_Retorna201()
        {
            //ARRANGE
            var produtoId = Guid.NewGuid();
            var produtoViewModel = new ObterProdutoViewModel
            {
                Id = produtoId,
                Nome = NomeProdutoPadrao,
                Descricao = DescricaoProdutoPadrao,
                Categoria = CategoriaProdutoPadrao,
                Preco = PrecoProdutoPadrao
            };
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProdutoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(produtoViewModel);

            //ACT
            var result = await _controller.Obter(produtoId);

            //ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(produtoViewModel, okResult.Value);
        }

        [Fact]
        public async Task Obter_ProdutoEncontrado_EnviaParaMediator()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProdutoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ObterProdutoViewModel());

            // Act
            await _controller.Obter(produtoId);

            // Assert
            _mediatorMock.Verify(
                m => m.Send(It.IsAny<ObterProdutoQuery>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        //CadastrarProduto
        [Fact]
        public async Task CadastrarProduto_CommandValido_Retorna201()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegistrarProdutoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MediatR.Unit.Value);

            var command = new RegistrarProdutoCommand
            {
                Nome = NomeProdutoPadrao,
                Descricao = DescricaoProdutoPadrao,
                Categoria = CategoriaProdutoPadrao,
                Preco = PrecoProdutoPadrao
            };

            // Act
            var result = await _controller.CadastrarProduto(command);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CadastrarProduto_CommandValido_EnviaParaMediator()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegistrarProdutoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MediatR.Unit.Value);

            var command = new RegistrarProdutoCommand
            {
                Nome = NomeProdutoPadrao,
                Descricao = DescricaoProdutoPadrao,
                Categoria = CategoriaProdutoPadrao,
                Preco = PrecoProdutoPadrao
            };

            // Act
            await _controller.CadastrarProduto(command);

            // Assert
            _mediatorMock.Verify(
                m => m.Send(It.IsAny<RegistrarProdutoCommand>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        //Atualizar Preco
         [Fact]
        public async Task AtualizarPreco_CommandValido_RetornaOk()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AtualizarPrecoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MediatR.Unit.Value);

            var command = new AtualizarPrecoCommand
            {
                Id = Guid.NewGuid(),
                Preco = PrecoProdutoPadrao
            };

            // Act
            var result = await _controller.AtualizarPreco(command);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AtualizarPreco_CommandValido_EnviaParaMediator()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AtualizarPrecoCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(MediatR.Unit.Value));

            var command = new AtualizarPrecoCommand
            {
                Id = Guid.NewGuid(),
                Preco = PrecoProdutoPadrao
            };

            // Act
            await _controller.AtualizarPreco(command);

            // Assert
            _mediatorMock.Verify(
                m => m.Send(It.IsAny<AtualizarPrecoCommand>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
