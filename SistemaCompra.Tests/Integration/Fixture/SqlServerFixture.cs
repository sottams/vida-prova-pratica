using Microsoft.EntityFrameworkCore;
using SistemaCompra.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace SistemaCompra.Tests.Integration.Fixture
{
    public class SqlServerFixture : IAsyncLifetime
    {
        private readonly MsSqlContainer _container;
        public SistemaCompraContext Context { get; private set; }
        public SqlServerFixture()
        {
            _container = new MsSqlBuilder().WithPassword("SistemaCompra@123").Build();
        }
        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            var options = new DbContextOptionsBuilder<SistemaCompraContext>()
                .UseSqlServer(_container.GetConnectionString())
                .Options;
            Context = new SistemaCompraContext(options);
            await Context.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await Context.DisposeAsync();
            await _container.DisposeAsync();
        }

    }
}
