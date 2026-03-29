using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using SistemaCompra.Infra.Data;
using SistemaCompra.Infra.Data.SolicitacaoCompra;
using SistemaCompra.Infra.Data.UoW;
using SistemaCompra.Tests.Integration.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Integration.Repository
{
    public class SolicitacaoCompraRepositoryIntegrationTest : IClassFixture<SqlServerFixture>, IAsyncLifetime
    {
        private readonly SistemaCompraContext _context;
        private readonly SolicitacaoCompraRepository _reposotory;
        private readonly UnitOfWork _unitOfWork;
        private IDbContextTransaction _transaction;
 
        public SolicitacaoCompraRepositoryIntegrationTest(SqlServerFixture fixture)
        {
            _context = fixture.Context;
            _reposotory = new SolicitacaoCompraRepository(_context);
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

        private const string UsuarioSolicitantePadrao = "Usuario Teste";
        private const string NomeFornecedorPadrao = "Fornecedor Teste";
        private const string NomeProdutoPadrao = "Produto Teste";
        private const string DescricaoProdutoPadrao = "Descricao Teste";
        private const string CategoriaProdutoPadrao = "Madeira";
        private const decimal PrecoProdutoPadrao = 100;

        [Fact]
        public async Task RegistrarCompra_PersisteSolicitacao()
        {
            //ARRANGE
            var solicitacao = new SolicitacaoCompra(UsuarioSolicitantePadrao, NomeFornecedorPadrao);
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            solicitacao.AdicionarItem(produto, 2);
            solicitacao.RegistrarCompra();

            //ACT
            _reposotory.RegistrarCompra(solicitacao);
            _unitOfWork.Commit();

            //ASSERT
            var salvo = await _context.SolicitacaoCompras.Include(s => s.Itens).FirstOrDefaultAsync(s => s.Id == solicitacao.Id);
            Assert.NotNull(salvo);
            Assert.Single(salvo.Itens);
        }

        [Fact]
        public async Task RegistrarCompra_PersisteTotalGeral()
        {
            //ARRANGE
            var solicitacao = new SolicitacaoCompra(UsuarioSolicitantePadrao, NomeFornecedorPadrao);
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            solicitacao.AdicionarItem(produto, 2);
            solicitacao.RegistrarCompra();

            //ACT
            _reposotory.RegistrarCompra(solicitacao);
            _unitOfWork.Commit();

            //ASSERT
            var salvo = await _context.SolicitacaoCompras.Include(s => s.Itens).FirstOrDefaultAsync(s => s.Id == solicitacao.Id);
            Assert.NotNull(salvo);
            Assert.Equal(200, salvo.TotalGeral.Value);
        }

        [Fact]
        public async Task RegistrarCompra_CondicaoPagamentoTrintaDias()
        {
            //ARRANGE
            var solicitacao = new SolicitacaoCompra(UsuarioSolicitantePadrao, NomeFornecedorPadrao);
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            solicitacao.AdicionarItem(produto, 10000);
            solicitacao.RegistrarCompra();

            //ACT
            _reposotory.RegistrarCompra(solicitacao);
            _unitOfWork.Commit();

            //ASSERT
            var salvo = await _context.SolicitacaoCompras.Include(s => s.Itens).FirstOrDefaultAsync(s => s.Id == solicitacao.Id);
            Assert.NotNull(salvo);
            Assert.Equal(30, salvo.CondicaoPagamento.Valor);
        }

        [Fact]
        public async Task RegistrarCompra_LancaExcessao()
        {
            //ARRANGE
            var solicitacao = new SolicitacaoCompra(UsuarioSolicitantePadrao, NomeFornecedorPadrao);
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            solicitacao.AdicionarItem(produto, 2);
            solicitacao.RegistrarCompra();
            _reposotory.RegistrarCompra(solicitacao);
            _unitOfWork.Commit();

            //ACT
            _reposotory.RegistrarCompra(solicitacao);
            //ASSERT
            Assert.Throws<DbUpdateException>(() => _unitOfWork.Commit());
        }
    }
}
