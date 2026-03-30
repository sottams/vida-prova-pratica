using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaCompra.API;
using SistemaCompra.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace SistemaCompra.Tests.Integration.Fixture
{
    public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder().Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<SistemaCompraContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<SistemaCompraContext>(options =>
                    options.UseSqlServer(_sqlContainer.GetConnectionString()));
            });
        }


        public async Task InitializeAsync()
        {
            await _sqlContainer.StartAsync();
            _ = CreateClient();

            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SistemaCompraContext>();
            await db.Database.MigrateAsync();
        }

        public new async Task DisposeAsync()
        {
            await _sqlContainer.StopAsync();
            await base.DisposeAsync();
        }
    }
}
