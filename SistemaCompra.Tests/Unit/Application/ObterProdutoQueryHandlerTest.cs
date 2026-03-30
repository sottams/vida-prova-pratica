using AutoMapper;
using Moq;
using SistemaCompra.Application.AutoMapper;
using SistemaCompra.Application.Produto.Query.ObterProduto;
using SistemaCompra.Domain.ProdutoAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCompra.Tests.Unit.Application
{
    public class ObterProdutoQueryHandlerTest
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ObterProdutoQueryHandler _handler;

        public ObterProdutoQueryHandlerTest()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();

            var config = new MapperConfiguration(cfg =>
                cfg.AddProfile<DomainToViewModelMappingProfile>());
            _mapper = config.CreateMapper();

            _handler = new ObterProdutoQueryHandler(
                _produtoRepositoryMock.Object,
                _mapper);
        }

        private const string NomeProdutoPadrao = "Produto Teste";
        private const string DescricaoProdutoPadrao = "Descricao Teste";
        private const string CategoriaProdutoPadrao = "Madeira";
        private const decimal PrecoProdutoPadrao = 100;

        public async Task Handle_ObterProduto_RetornaProdutoViewModel()
        {
            //ARRANGE
            var produto = new Produto(NomeProdutoPadrao, DescricaoProdutoPadrao, CategoriaProdutoPadrao, PrecoProdutoPadrao);
            _produtoRepositoryMock
                .Setup(p => p.ObterAsync(produto.Id))
                .ReturnsAsync(produto);
            var query = new ObterProdutoQuery { Id = produto.Id };

            //ACT
            var result = await _handler.Handle(query, CancellationToken.None);

            //ASSERT
            Assert.NotNull(result);
            Assert.Equal(NomeProdutoPadrao, result.Nome);
            Assert.Equal(DescricaoProdutoPadrao, result.Descricao);
            Assert.Equal(CategoriaProdutoPadrao, result.Categoria);
            Assert.Equal(PrecoProdutoPadrao, result.Preco);
        }

        [Fact]
        public async Task Handle_ProdutoNaoEncontrado_RetornaNull()
        {
            //ARRANGE
            var query = new ObterProdutoQuery { Id = Guid.NewGuid() };

            //ACT
            var result = await _handler.Handle(query, CancellationToken.None);

            //ASSERT
            Assert.Null(result);
        }
    }
}
