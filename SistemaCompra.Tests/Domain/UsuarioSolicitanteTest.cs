using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Domain
{
    public class UsuarioSolicitanteTest
    {
        private readonly UsuarioSolicitante _usuarioSolicitante;
        public UsuarioSolicitanteTest()
        {
            _usuarioSolicitante = new UsuarioSolicitante("Usuario Teste");
        }

        // Teste para o construtor
        [Fact]
        public void Construtor_ValorValido()
        {
            Assert.Equal("Usuario Teste", _usuarioSolicitante.Nome);
        }
        [Fact]
        public void Construtor_ValorVazio()
        {
            Assert.Throws<ArgumentNullException>(() => new UsuarioSolicitante(""));
        }
        [Fact]
        public void Construtor_ValorIncorreto()
        {
            Assert.Throws<BusinessRuleException>(() => new UsuarioSolicitante("A"));
        }
    }
}
