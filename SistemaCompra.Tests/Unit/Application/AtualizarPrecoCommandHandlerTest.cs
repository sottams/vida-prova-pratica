using Moq;
using SistemaCompra.Application.Produto.Command.AtualizarPreco;
using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Infra.Data.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Unit.Application
{
    public class AtualizarPrecoCommandHandlerTest
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AtualizarPrecoCommandHandler _handler;

        public AtualizarPrecoCommandHandlerTest()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(u => u.Commit()).Returns(true);

            _handler = new AtualizarPrecoCommandHandler(
                _produtoRepositoryMock.Object,
                _unitOfWorkMock.Object
            );
        }

        private const string NomeProdutoPadrao = "Produto Teste";
        private const string DescricaoProdutoPadrao = "Descricao Teste";
        private const string CategoriaProdutoPadrao = "Madeira";
        private const decimal PrecoAtual = 100;
        private const decimal Preconovo = 200;

        [Fact]
        public async Task Handle_AtualizarPrecoAsync_AtualizaPrecoNoRepository()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoAtual);
            _produtoRepositoryMock
                .Setup(p => p.ObterAsync(produto.Id))
                .ReturnsAsync(produto);

            var command = new AtualizarPrecoCommand
            {
                Id = produto.Id,
                Preco = Preconovo
            };

            //ACT
            await _handler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtoRepositoryMock.Verify(p => p.Atualizar(It.Is<Produto>(prod =>
                prod.Id == produto.Id &&
                prod.Preco.Value == Preconovo
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_AtualizarPrecoAsync_ExecutaCommit()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoAtual);
            _produtoRepositoryMock
                .Setup(p => p.ObterAsync(produto.Id))
                .ReturnsAsync(produto);

            var command = new AtualizarPrecoCommand
            {
                Id = produto.Id,
                Preco = Preconovo
            };

            //ACT
            await _handler.Handle(command, CancellationToken.None);

            //ASSERT
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_AtualizarPrecoAsync_ProdutoNaoEncontrado_NaoAtualiza()
        {
            //ARRANGE
            var produtoId = Guid.NewGuid();
            _produtoRepositoryMock
                .Setup(p => p.ObterAsync(produtoId))
                .ReturnsAsync((Produto)null);
            var command = new AtualizarPrecoCommand
            {
                Id = produtoId,
                Preco = Preconovo
            };

            //ACT
            await _handler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtoRepositoryMock.Verify(p => p.Atualizar(It.IsAny<Produto>()), Times.Never);
        }

        [Fact]
        public async Task Handle_AtualizarPrecoAsync_ProdutoNaoEncontrado_NaoExecutaCommit()
        {
            //ARRANGE
            var produtoId = Guid.NewGuid();
            _produtoRepositoryMock
                .Setup(p => p.ObterAsync(produtoId))
                .ReturnsAsync((Produto)null);
            var command = new AtualizarPrecoCommand
            {
                Id = produtoId,
                Preco = Preconovo
            };

            //ACT
            await _handler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtoRepositoryMock.Verify(p => p.Atualizar(It.IsAny<Produto>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }

        [Fact]
        public async Task Handle_AtualizarprecoAsync_CommitFalha()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoAtual);
            _produtoRepositoryMock
                .Setup(p => p.ObterAsync(produto.Id))
                .ReturnsAsync(produto);

            _unitOfWorkMock
               .Setup(u => u.Commit())
               .Throws(new InvalidOperationException("Ocorreu um erro ao salvar os dados."));

            var command = new AtualizarPrecoCommand
            {
                Id = produto.Id,
                Preco = Preconovo
            };

            //ACT && ASSERT
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None));


        }

        [Fact]
        public async Task Handle_AtualizarprecoAsync_LancaBusinessRuleException()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoAtual);
            produto.Suspender();

            _produtoRepositoryMock
                .Setup(p => p.ObterAsync(produto.Id))
                .ReturnsAsync(produto);

            var command = new AtualizarPrecoCommand
            {
                Id = produto.Id,
                Preco = Preconovo
            };

            //ACT & ASSERT
            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _handler.Handle(command, CancellationToken.None));
        }
    }
}
