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
    public class ProdutoControllerIntegrationTest : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;

        public ProdutoControllerIntegrationTest(ApiFactory factory)
        {
           _client = factory.CreateClient();
        }

        private StringContent CriarConteudo(object obj) =>
            new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                "application/json");

        private const string NomeProdutoPadrao = "Produto Teste";
        private const string DescricaoProdutoPadrao = "Descricao Teste";
        private const string CategoriaProdutoPadrao = "Madeira";
        private const decimal PrecoProdutoPadrao = 100;

        [Fact]
        public async Task CadastrarProduto_CommandValido_Retorna201()
        {
            // Arrange
            var command = new
            {
                Nome = NomeProdutoPadrao,
                Descricao = DescricaoProdutoPadrao,
                Categoria = CategoriaProdutoPadrao,
                Preco = PrecoProdutoPadrao
            };

            // Act
            var response = await _client.PostAsync("/produto/cadastro", CriarConteudo(command));

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CadastrarProduto_NomeVazio_Retorna400()
        {
            // Arrange
            var command = new
            {
                Nome = "",
                Descricao = DescricaoProdutoPadrao,
                Categoria = CategoriaProdutoPadrao,
                Preco = PrecoProdutoPadrao
            };

            // Act
            var response = await _client.PostAsync("/produto/cadastro", CriarConteudo(command));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CadastrarProduto_CategoriaInvalida_Retorna400()
        {
            // Arrange
            var command = new
            {
                Nome = NomeProdutoPadrao,
                Descricao = DescricaoProdutoPadrao,
                Categoria = "CategoriaInvalida",
                Preco = PrecoProdutoPadrao
            };

            // Act
            var response = await _client.PostAsync("/produto/cadastro", CriarConteudo(command));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AtualizarPreco_ProdutoExistente_RetornaOk()
        {
            // Arrange
            var cadastroCommand = new
            {
                Nome = NomeProdutoPadrao,
                Descricao = DescricaoProdutoPadrao,
                Categoria = CategoriaProdutoPadrao,
                Preco = PrecoProdutoPadrao
            };
            await _client.PostAsync("/produto/cadastro", CriarConteudo(cadastroCommand));

            // Act
            var atualizarCommand = new
            {
                Id = Guid.NewGuid(), 
                Preco = 200m
            };
            var response = await _client.PutAsync("/produto/atualiza-preco", CriarConteudo(atualizarCommand));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
