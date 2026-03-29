using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolicitacaoAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;


namespace SistemaCompra.Infra.Data.SolicitacaoCompra
{
    public class ItemConfiguration : IEntityTypeConfiguration<SolicitacaoAgg.Item>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoAgg.Item> builder)
        {
            builder.ToTable("Item");
            builder.Ignore(i => i.Subtotal);
        }
    }
}
