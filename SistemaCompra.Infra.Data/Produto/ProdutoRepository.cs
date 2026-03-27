using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ProdutoAgg = SistemaCompra.Domain.ProdutoAggregate;

namespace SistemaCompra.Infra.Data.Produto
{
    public class ProdutoRepository : ProdutoAgg.IProdutoRepository
    {
        private readonly SistemaCompraContext context;

        public ProdutoRepository(SistemaCompraContext context)  {
            this.context = context;
        }

        public void Atualizar(ProdutoAgg.Produto entity)
        {
            context.Set<ProdutoAgg.Produto>().Update(entity);
        }

        public void Excluir(ProdutoAgg.Produto entity)
        {
            context.Set<ProdutoAgg.Produto>().Remove(entity);
        }

        public async Task<ProdutoAgg.Produto> ObterAsync(Guid id)
        {
            return await context.Set<ProdutoAgg.Produto>().FirstOrDefaultAsync(c=> c.Id == id);
        }

        public async Task RegistrarAsync(ProdutoAgg.Produto entity)
        {
            await context.Set<ProdutoAgg.Produto>().AddAsync(entity);
        }
    }
}
