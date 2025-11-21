using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Bernhoeft.GRT.Core.EntityFramework.Domain.Interfaces;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.ContractWeb.Infra.Persistence.SqlServer.ContractStore.Mappings;
using Microsoft.Extensions.Hosting;

namespace Bernhoeft.GRT.Teste.IntegrationTests.Fixtures
{
    public class WebApplicationFactoryFixture : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            
            builder.ConfigureServices(services =>
            {
                // Remove o DbContext existente
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IContext));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Remove todos os serviços relacionados ao DbContext
                var dbContextDescriptors = services.Where(d => 
                    d.ServiceType == typeof(IContext) ||
                    (d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)))
                    .ToList();

                foreach (var desc in dbContextDescriptors)
                {
                    services.Remove(desc);
                }

                // Adiciona um novo DbContext em memória para testes com nome único
                var dbName = "TestDb_" + Guid.NewGuid().ToString();
                services.AddBernhoeftDbContext<AvisoMap>((serviceProvider, options) =>
                {
                    options.UseInMemoryDatabase(dbName);
                });

                // Registra os serviços necessários
                services.RegisterServicesFromAssemblyContaining<AvisoMap>();
            });
        }
    }
}

