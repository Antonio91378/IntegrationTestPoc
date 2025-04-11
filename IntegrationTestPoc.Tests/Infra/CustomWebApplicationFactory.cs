using IntegrationTestPoc.Tests.Helper;
using IntegrationTestPoc.Util;
using IntegrationTestProc.Infra;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTestPoc.Tests.Infra;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{   
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        
        builder.ConfigureServices((context, services) =>
        {
            builder.UseEnvironment("test");
            var configuration = context.Configuration;
            var appConfiguration = new AppConfiguration(configuration);

            var connectionString = configuration.GetConnectionString("SqlServer");
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<Context>));

            services.Remove(dbContextDescriptor!);

            dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(Context));
            services.Remove(dbContextDescriptor!);

             // Adiciona o TestStarterHelper ao contêiner de serviços
            services.AddScoped<TestStarterHelper>(provider =>
            {
                return new TestStarterHelper(appConfiguration);
            });


            services.AddDbContext<Context>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);

            // // Mock para IServicoEmail
            // services.RemoveAll(typeof(IServicoEmail));
            // var mockServicoEmail = Substitute.For<IServicoEmail>();
            // services.AddSingleton(mockServicoEmail);
        });

    }
}