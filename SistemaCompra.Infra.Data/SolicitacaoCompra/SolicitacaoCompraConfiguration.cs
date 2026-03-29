using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolicitacaoAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;

namespace SistemaCompra.Infra.Data.SolicitacaoCompra
{
    public class SolicitacaoCompraConfiguration : IEntityTypeConfiguration<SolicitacaoAgg.SolicitacaoCompra>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoAgg.SolicitacaoCompra> builder)
        {
            builder.ToTable("SolicitacaoCompra");

            builder.OwnsOne(c => c.UsuarioSolicitante, b => b.Property(u => u.Nome).HasColumnName("UsuarioSolicitante"));
            builder.OwnsOne(c => c.NomeFornecedor, b => b.Property(u => u.Nome).HasColumnName("NomeFornecedor"));
            builder.OwnsOne(c => c.TotalGeral, b => b.Property(u => u.Value).HasColumnName("TotalGeral"));
            builder.OwnsOne(c => c.CondicaoPagamento, b => b.Property(u => u.Valor).HasColumnName("CondicaoPagamento"));
            builder.HasMany(c => c.Itens).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
