using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Unit.Domain.SolicitacaoCompraAggregate
{
    public class CondicaoPagamentoTest
    {
        private readonly CondicaoPagamento _condicaoPagamento;

        public CondicaoPagamentoTest()
        {
            _condicaoPagamento = new CondicaoPagamento(30);
        }

        [Fact]
        public void Construtor_ValorValido()
        {
            Assert.Equal(30, _condicaoPagamento.Valor);
        }

        [Fact]
        public void Construtor_ValorInvalido()
        {
            Assert.Throws<BusinessRuleException>(() => new CondicaoPagamento(15));
        }
    }
}
