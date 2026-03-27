using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaCompra.Domain.ProdutoAggregate
{
    public interface IProdutoRepository
    {
        Task<Produto> ObterAsync(Guid id);
        Task RegistrarAsync(Produto entity);
        void Atualizar(Produto entity);
        void Excluir(Produto entity);
    }
}
