using Moq;
using SistemaCompra.Application.Produto.Command.RegistrarProduto;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Infra.Data.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Unit.Application
{
    public class RegistrarProdutoCommandHandlerTest
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RegistrarProdutoCommandHandler _handler;

        public RegistrarProdutoCommandHandlerTest()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(u => u.Commit()).Returns(true);

            _handler = new RegistrarProdutoCommandHandler(
                _produtoRepositoryMock.Object,
                _unitOfWorkMock.Object
            );
        }

        private const string NomeProdutoPadrao = "Produto Teste";
        private const string DescricaoProdutoPadrao = "Descricao Teste";
        private const string CategoriaProdutoPadrao = "Madeira";
        private const decimal PrecoProdutoPadrao = 100;

        [Fact]
        public async Task Handle_RegistrarAsync_RegistraNoRepository()
        {
            //ARRANGE
            var command = new RegistrarProdutoCommand
            {
                Nome = NomeProdutoPadrao,
                Descricao = DescricaoProdutoPadrao,
                Categoria = CategoriaProdutoPadrao,
                Preco = PrecoProdutoPadrao
            };

            //ACT
            await _handler.Handle(command, CancellationToken.None);

            //ASSERT
            _produtoRepositoryMock.Verify(p => p.RegistrarAsync(It.Is<Produto>(prod =>
                prod.Nome == NomeProdutoPadrao &&
                prod.Descricao == DescricaoProdutoPadrao &&
                prod.Categoria.ToString() == CategoriaProdutoPadrao &&
                prod.Preco.Value == PrecoProdutoPadrao
            )), Times.Once);

            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_RegistrarAsync_CommitFalha()
        {
            //ARRANGE
            _unitOfWorkMock
                .Setup(u => u.Commit())
                .Throws(new InvalidOperationException("Ocorreu um erro ao salvar os dados."));

            var command = new RegistrarProdutoCommand
            {
                Nome = NomeProdutoPadrao,
                Descricao = DescricaoProdutoPadrao,
                Categoria = CategoriaProdutoPadrao,
                Preco = PrecoProdutoPadrao
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

    }
}
