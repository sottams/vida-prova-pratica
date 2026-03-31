using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using SistemaCompra.Application.SolicitacaoCompra.Events;
using SistemaCompra.Domain.Events;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using SistemaCompra.Infra.Data;
using SistemaCompra.Infra.Data.Events;
using SistemaCompra.Infra.Data.Produto;
using SistemaCompra.Infra.Data.SolicitacaoCompra;
using SistemaCompra.Infra.Data.UoW;
using System;

namespace SistemaCompra.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }



        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var assembly = AppDomain.CurrentDomain.Load("SistemaCompra.Application");
            services.AddMediatR(assembly);
            services.AddAutoMapper(assembly);
            services.AddSignalR();

            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<ISolicitacaoCompraRepository, SolicitacaoCompraRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<SistemaCompraContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Prova TI VIDA", Version = "v1" });
            });
            
            if (!Environment.IsEnvironment("Testing"))
            {
                var rabbitConnection = new ConnectionFactory
            {
                HostName = Configuration["RabbitMQ:Host"],
                UserName = Configuration["RabbitMQ:User"],
                Password = Configuration["RabbitMQ:Password"]
            }.CreateConnection();

            services.AddSingleton<IConnection>(rabbitConnection);
            services.AddScoped<IDomainEventHandler<CompraRegistrada>, PublicarMensagemCompraRegistradaHandler>();
            }
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IDomainEventHandler<CompraRegistrada>, EnviarEmailCompraRegistradaHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<Middleware.ExceptionMiddleware>();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prova TI VIDA V1");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
