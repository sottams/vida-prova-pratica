using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.ProdutoAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Unit.Domain.ProdutoAggregate
{
    public class ProdutoTest
    {
        private readonly Produto _produto;
        public ProdutoTest()
        {
            _produto = new Produto("Produto Teste", "Descrição Teste", "Madeira", 100);
        }

        // Teste para o construtor
        [Fact]
        public void Construtor_PropriedadesIniciais()
        {
            Assert.Equal("Produto Teste", _produto.Nome);
            Assert.Equal("Descrição Teste", _produto.Descricao);
            Assert.Equal("Madeira", _produto.Categoria.ToString());
            Assert.Equal(100, _produto.Preco.Value);
        }
        [Fact]
        public void Construtor_NomeVazio()
        {
            Assert.Throws<ArgumentNullException>(() => new Produto("", "Descrição Teste", "Madeira", 100));
        }
        [Fact]
        public void Construtor_DescricaoVazia()
        {
            Assert.Throws<ArgumentNullException>(() => new Produto("Produto Teste", "", "Madeira", 100));
        }

        //Teste para Ativar
        [Fact]
        public void Ativar_ProdutoAtivo()
        {
            _produto.Ativar();
            Assert.Equal(Situacao.Ativo, _produto.Situacao);
        }

        //Teste para Suspender
        [Fact]
        public void Suspender_Produto()
        {
            _produto.Suspender();
            Assert.Equal(Situacao.Suspenso, _produto.Situacao);

        }

        //Teste para AtualizarPreco
        [Fact]
        public void AtualizarPreco_SituacaoAtiva()
        {
            _produto.AtualizarPreco(150);
            Assert.Equal(150, _produto.Preco.Value);
        }
        [Fact]
        public void AtualizarPreco_SituacaoSuspensa()
        {
            _produto.Suspender();
            Assert.Throws<BusinessRuleException>(() => _produto.AtualizarPreco(150));
        }
    }
}
