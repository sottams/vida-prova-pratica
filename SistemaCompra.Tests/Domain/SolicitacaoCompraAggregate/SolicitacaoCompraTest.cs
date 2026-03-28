using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using System;
using System.Collections.Generic;

namespace SistemaCompra.Tests.Domain.SolicitacaoCompraAggregate
{
    public class SolicitacaoCompraTests
    {
        private readonly SolicitacaoCompra _solicitacaoCompra;
        public SolicitacaoCompraTests()
        {
            _solicitacaoCompra = new SolicitacaoCompra("Usuario Teste", "Fornecedor Teste");
        }

        //Testes para o construtor
        [Fact]
        public void Construtor_PropriedadesIniciais()
        {
            Assert.Equal("Usuario Teste", _solicitacaoCompra.UsuarioSolicitante.Nome);
            Assert.Equal("Fornecedor Teste", _solicitacaoCompra.NomeFornecedor.Nome);
            Assert.Empty(_solicitacaoCompra.Itens);
            Assert.Null(_solicitacaoCompra.TotalGeral);
            Assert.Null(_solicitacaoCompra.CondicaoPagamento);
            Assert.Equal(SistemaCompra.Domain.SolicitacaoCompraAggregate.Situacao.Solicitado, _solicitacaoCompra.Situacao);
        }


        // Testes para o método AdicionarItem
        [Fact]
        public void AdicionarItem_AdicionaItemNaLista()
        {
            var produto = new Produto("Produto Teste", "Descrição Teste", "Madeira", 1);

            _solicitacaoCompra.AdicionarItem(produto, 2);

            Assert.NotEmpty(_solicitacaoCompra.Itens);
        }

        [Fact]
        public void AdicionarItem_AdicionarMultiplosItens()
        {
            var produto1 = new Produto("Produto Teste 1", "Descrição Teste 1", "Madeira", 1);
            var produto2 = new Produto("Produto Teste 2", "Descrição Teste 2", "Juncao", 2);

            _solicitacaoCompra.AdicionarItem(produto1, 2);
            _solicitacaoCompra.AdicionarItem(produto2, 3);

            Assert.Equal(2, _solicitacaoCompra.Itens.Count);
        }

        [Fact]
        public void AdicionarItem_ListaVazia()
        {
            Assert.Throws<ArgumentNullException>(() => _solicitacaoCompra.AdicionarItem(null, 2));
        }

        // Testes para o método RegistrarCompra
        [Fact]
        public void RegistrarCompra_CondicaoPagamentoTrintaDias()
        {
            var produto = new Produto("Produto Caro", "Descrição Caro", "Juncao", 10000);
            _solicitacaoCompra.AdicionarItem(produto, 6); // Total: 60000
            _solicitacaoCompra.RegistrarCompra();

            Assert.Equal(60000, _solicitacaoCompra.TotalGeral.Value);
            Assert.Equal(30, _solicitacaoCompra.CondicaoPagamento.Valor);
        }
        [Fact]
        public void RegistrarCompra_CondicaoPagamentoPadrao()
        {
            var produto = new Produto("Produto Barato", "Descrição Barato", "Juncao", 100);
            _solicitacaoCompra.AdicionarItem(produto, 2); // Total: 200
            _solicitacaoCompra.RegistrarCompra();

            Assert.Equal(200, _solicitacaoCompra.TotalGeral.Value);
            Assert.Equal(0, _solicitacaoCompra.CondicaoPagamento.Valor);
        }
        [Fact]
        public void RegistrarCompra_SemItens()
        {
            Assert.Throws<BusinessRuleException>(() => _solicitacaoCompra.RegistrarCompra());
        }
     }
}
