using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Infra.Data;
using SistemaCompra.Infra.Data.Produto;
using SistemaCompra.Infra.Data.UoW;
using SistemaCompra.Tests.Integration.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Integration.Repository
{
    public class ProdutoRepositoryIntegrationTest : IClassFixture<SqlServerFixture>, IAsyncLifetime
    {
        private readonly SistemaCompraContext _context;
        private readonly ProdutoRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private IDbContextTransaction _transaction;

        public ProdutoRepositoryIntegrationTest(SqlServerFixture fixture)
        {
            _context = fixture.Context;
            _repository = new ProdutoRepository(_context);
            _unitOfWork = new UnitOfWork(_context);
        }
        public async Task InitializeAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task DisposeAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        private const string NomeProdutoPadrao = "Produto Teste";
        private const string DescricaoProdutoPadrao = "Descricao Teste";
        private const string CategoriaProdutoPadrao = "Madeira";
        private const decimal PrecoProdutoPadrao = 100;

        [Fact]
        public async Task RegistrarAsync_PersisteProduto()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);

            //ACT
            await _repository.RegistrarAsync(produto);
            _unitOfWork.Commit();

            //ASSERT
            var salvo = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produto.Id);
            Assert.NotNull(salvo);
        }

        [Fact]
        public async Task ObterAsync_RetornaProduto()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);

            //ACT
            await _repository.RegistrarAsync(produto);
            _unitOfWork.Commit();

            //ASSERT
            var obtido = await _repository.ObterAsync(produto.Id);
            Assert.NotNull(obtido);
            Assert.Equal(produto.Id, obtido.Id);
        }

        [Fact]
        public async Task RegistrarAsync_PrecoMapeado()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);

            //ACT
            await _repository.RegistrarAsync(produto);
            _unitOfWork.Commit();

            //ASSERT
            var salvo = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produto.Id);
            Assert.Equal(PrecoProdutoPadrao, salvo.Preco.Value);
        }

        [Fact]
        public async Task ObterAsync_RetornaNullQuandoNaoExiste()
        {
            //ARRANGE
            var idInexistente = Guid.NewGuid();

            //ASSERT
            var obtido = await _repository.ObterAsync(idInexistente);
            Assert.Null(obtido);
        }
        [Fact]
        public async Task Atualizar_PersisteAlteracao()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            await _repository.RegistrarAsync(produto);
            _unitOfWork.Commit();
            
            //ACT
            produto.AtualizarPreco(150);
            _repository.Atualizar(produto);
            _unitOfWork.Commit();
            
            //ASSERT
            var salvo = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produto.Id);
            Assert.Equal(150, salvo.Preco.Value);
        }

        [Fact]
        public async Task Excluir_RemoverProduto()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            await _repository.RegistrarAsync(produto);
            _unitOfWork.Commit();

            //ACT
            _repository.Excluir(produto);
            _unitOfWork.Commit();

            //ASSERT
            var excluido = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produto.Id);
            Assert.Null(excluido);
        }

        [Fact]
        public async Task RegistrarAsync_LancaExcessao()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            await _repository.RegistrarAsync(produto);
            _unitOfWork.Commit();
            
            //ACT
            await _repository.RegistrarAsync(produto);
            
            //ASSERT
            Assert.Throws<DbUpdateException>(() => _unitOfWork.Commit());
        }
    }
}
