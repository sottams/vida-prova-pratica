using Moq;
using SistemaCompra.Application.Produto.Command.RegistrarProduto;
using SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra;
using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Events;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using SistemaCompra.Infra.Data.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Unit.Application
{
    public class RegistrarCompraCommandHandlerTest
    {
        private readonly Mock<ISolicitacaoCompraRepository> _solicitacaoRepositoryMock;
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly Mock<IDomainEventDispatcher> _dispatcherMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RegistrarCompraCommandHandler _handler;

        public RegistrarCompraCommandHandlerTest()
        {
            _solicitacaoRepositoryMock = new Mock<ISolicitacaoCompraRepository>();
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _dispatcherMock = new Mock<IDomainEventDispatcher>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(u => u.Commit()).Returns(true);

            _handler = new RegistrarCompraCommandHandler(
                _solicitacaoRepositoryMock.Object,
                _produtoRepositoryMock.Object,
                _dispatcherMock.Object,
                _unitOfWorkMock.Object
            );
        }

        private const string UsuarioSolicitantePadrao = "Usuario Teste";
        private const string NomeFornecedorPadrao = "Fornecedor Teste";
        private const string NomeProdutoPadrao = "Produto Teste";
        private const string DescricaoProdutoPadrao = "Descricao Teste";
        private const string CategoriaProdutoPadrao = "Madeira";
        private const decimal PrecoProdutoPadrao = 100;
        private const int QuantidadeItem = 2;

        [Fact]
        public async Task Handle_RegistrarCompra_RegistraNoRepository()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            _produtoRepositoryMock
                .Setup(p => p.ObterAsync(produto.Id))
                .ReturnsAsync(produto);
            var command = new RegistrarCompraCommand
            {
                UsuarioSolicitante = UsuarioSolicitantePadrao,
                NomeFornecedor = NomeFornecedorPadrao,
                Itens = new List<ItemCommand>
                {
                    new ItemCommand
                    {
                        IdProduto = produto.Id,
                        Quantidade = QuantidadeItem
                    }
                }
            };

            //ACT
            await _handler.Handle(command, CancellationToken.None);

            //ASSERT
            _solicitacaoRepositoryMock.Verify(
                r => r.RegistrarCompra(It.Is<SolicitacaoCompra>(s =>
                    s.UsuarioSolicitante.Nome == UsuarioSolicitantePadrao &&
                    s.NomeFornecedor.Nome == NomeFornecedorPadrao &&
                    s.Itens.Count == 1 &&
                    s.TotalGeral.Value == PrecoProdutoPadrao * QuantidadeItem)),
                Times.Once
                );
        }

        [Fact]
        public async Task Handle_RegistrarCompra_ExecutaCommit()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            _produtoRepositoryMock
                .Setup(p => p.ObterAsync(produto.Id))
                .ReturnsAsync(produto);
            var command = new RegistrarCompraCommand
            {
                UsuarioSolicitante = UsuarioSolicitantePadrao,
                NomeFornecedor = NomeFornecedorPadrao,
                Itens = new List<ItemCommand>
                {
                    new ItemCommand
                    {
                        IdProduto = produto.Id,
                        Quantidade = QuantidadeItem
                    }
                }
            };

            //ACT
            await _handler.Handle(command, CancellationToken.None);

            //ASSERT
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_RegistrarCompra_SemItens()
        {
            //ARRANGE
            var command = new RegistrarCompraCommand
            {
                UsuarioSolicitante = UsuarioSolicitantePadrao,
                NomeFornecedor = NomeFornecedorPadrao,
                Itens = new List<ItemCommand>()
            };

            //ACT & ASSERT
            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _handler.Handle(command, CancellationToken.None));

            _solicitacaoRepositoryMock.Verify(
                r => r.RegistrarCompra(It.IsAny<SolicitacaoCompra>()),
                Times.Never);
            
            _unitOfWorkMock.Verify(
                u => u.Commit(), 
                Times.Never);
            
            _dispatcherMock.Verify(
                d => d.Dispatch(It.IsAny<CompraRegistrada>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_RegistrarCompra_CommitFalha()
        {
            //ARRANGE
            _unitOfWorkMock
                .Setup(u => u.Commit())
                .Throws(new InvalidOperationException("Ocorreu um erro ao salvar os dados."));

            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            _produtoRepositoryMock
                .Setup(p => p.ObterAsync(produto.Id))
                .ReturnsAsync(produto);
            var command = new RegistrarCompraCommand
            {
                UsuarioSolicitante = UsuarioSolicitantePadrao,
                NomeFornecedor = NomeFornecedorPadrao,
                Itens = new List<ItemCommand>
                {
                    new ItemCommand
                    {
                        IdProduto = produto.Id,
                        Quantidade = QuantidadeItem
                    }
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_RegistrarCompra_DisparaEvento()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            _produtoRepositoryMock
                .Setup(p => p.ObterAsync(produto.Id))
                .ReturnsAsync(produto);
            var command = new RegistrarCompraCommand
            {
                UsuarioSolicitante = UsuarioSolicitantePadrao,
                NomeFornecedor = NomeFornecedorPadrao,
                Itens = new List<ItemCommand>
                {
                    new ItemCommand
                    {
                        IdProduto = produto.Id,
                        Quantidade = QuantidadeItem
                    }
                }
            };

            //ACT
            await _handler.Handle(command, CancellationToken.None);

            //ASSERT
            _dispatcherMock.Verify(d => d.Dispatch(It.Is<CompraRegistrada>(e =>
                e.UsuarioSolicitante == UsuarioSolicitantePadrao &&
                e.NomeFornecedor == NomeFornecedorPadrao &&
                e.Itens.Count == 1 &&
                e.ValorTotal == PrecoProdutoPadrao * QuantidadeItem)), Times.Once);
        }
    }
}
