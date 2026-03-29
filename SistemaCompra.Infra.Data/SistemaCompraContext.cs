using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaCompra.Domain.Core;
using SistemaCompra.Infra.Data.Produto;
using SistemaCompra.Infra.Data.SolicitacaoCompra;
using ProdutoAgg = SistemaCompra.Domain.ProdutoAggregate;
using SolicitacaoAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;

namespace SistemaCompra.Infra.Data
{
    public class SistemaCompraContext : DbContext
    {
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public SistemaCompraContext(DbContextOptions options) : base(options) { }
        public DbSet<ProdutoAgg.Produto> Produtos { get; set; }
        public DbSet<SolicitacaoAgg.SolicitacaoCompra> SolicitacaoCompras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            modelBuilder.ApplyConfiguration(new SolicitacaoCompraConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS01;Database=SistemaCompraDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
            
        }
    }
}
