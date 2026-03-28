using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Domain.SolicitacaoCompraAggregate
{
    public class ItemTest
    {
        private readonly Item _item;
        public ItemTest()
        {
            var produto = new Produto("Produto Teste", "Descrição Teste", "Madeira", 100);
            _item = new Item(produto, 2);
        }

        // Teste para o construtor
        [Fact]
        public void Construtor_PropriedadesIniciais()
        {
            Assert.Equal("Produto Teste", _item.Produto.Nome);
            Assert.Equal(2, _item.Qtde);
            Assert.Equal(200, _item.Subtotal.Value);
        }
        [Fact]
        public void Construtor_ProdutoNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new Item(null, 2));
        }
    }
}
