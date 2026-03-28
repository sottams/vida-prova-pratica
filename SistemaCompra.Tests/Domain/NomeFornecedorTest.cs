using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Domain
{
    public class NomeFornecedorTest
    {
        private readonly NomeFornecedor _nomeFornecedor;
        public NomeFornecedorTest()
        {
            _nomeFornecedor = new NomeFornecedor("Fornecedor Teste");
        }

        // Teste para o construtor
        [Fact]
        public void Construtor_ValorValido()
        {
            Assert.Equal("Fornecedor Teste", _nomeFornecedor.Nome);
        }
        [Fact]
        public void Construtor_ValorVazio()
        {
            Assert.Throws<ArgumentNullException>(() => new NomeFornecedor(""));
        }
        [Fact]
        public void Construtor_ValorIncorreto()
        {
            Assert.Throws<BusinessRuleException>(() => new NomeFornecedor("A"));
        }
    }
}
