using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolicitacaoAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;


namespace SistemaCompra.Infra.Data.SolicitacaoCompra
{
    public class SolicitacaoCompraRepository : SolicitacaoAgg.ISolicitacaoCompraRepository
    {
        private readonly SistemaCompraContext _context;
        public SolicitacaoCompraRepository(SistemaCompraContext context)
        {
            _context = context;
        }

        public void RegistrarCompra(SolicitacaoAgg.SolicitacaoCompra entity)
        {
            _context.Set<SolicitacaoAgg.SolicitacaoCompra>().Add(entity);
        }
    }
}
