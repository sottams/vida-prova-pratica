using Microsoft.Extensions.DependencyInjection;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Infra.Data;
using SistemaCompra.Tests.Integration.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Integration.Controller
{
    public class SolicitacaoCompraControllerIntegrationTest : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;
        private readonly ApiFactory _factory;

        public SolicitacaoCompraControllerIntegrationTest(ApiFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private StringContent CriarConteudo(object obj) =>
            new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                "application/json");

        private const string UsuarioSolicitantePadrao = "Usuario Teste";
        private const string NomeFornecedorPadrao = "Fornecedor Teste";
        private const string NomeProdutoPadrao = "Produto Teste";
        private const string DescricaoProdutoPadrao = "Descricao Teste";
        private const string CategoriaProdutoPadrao = "Madeira";
        private const decimal PrecoProdutoPadrao = 100;
        private async Task<Guid> CadastrarProduto()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SistemaCompraContext>();

            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            db.Set<Produto>().Add(produto);
            await db.SaveChangesAsync();

            return produto.Id;
        }

        [Fact]
        public async Task RegistrarCompra_CommandValido_Retorna201()
        {
            // Arrange
            var produtoId = await CadastrarProduto();

            var command = new
            {
                UsuarioSolicitante = UsuarioSolicitantePadrao,
                NomeFornecedor = NomeFornecedorPadrao,
                Itens = new[]
                {
                    new { IdProduto = produtoId, Quantidade = 2 }
                }
            };

            // Act
            var response = await _client.PostAsync("/solicitacao-compra/registrar", CriarConteudo(command));

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task RegistrarCompra_SemItens_Retorna400()
        {
            // Arrange
            var command = new
            {
                UsuarioSolicitante = UsuarioSolicitantePadrao,
                NomeFornecedor = NomeFornecedorPadrao,
                Itens = Array.Empty<object>()
            };

            // Act
            var response = await _client.PostAsync("/solicitacao-compra/registrar", CriarConteudo(command));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RegistrarCompra_UsuarioSolicitanteVazio_Retorna400()
        {
            // Arrange
            var command = new
            {
                UsuarioSolicitante = "",
                NomeFornecedor = NomeFornecedorPadrao,
                Itens = new[]
                {
                    new { IdProduto = Guid.NewGuid(), Quantidade = 1 }
                }
            };

            // Act
            var response = await _client.PostAsync("/solicitacao-compra/registrar", CriarConteudo(command));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
