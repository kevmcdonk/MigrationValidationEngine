using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PnP.Core.Auth;
using PnP.Core.Auth.Services.Builder.Configuration;
using PnP.Core.Services;
using PnP.Core.Services.Builder;
using PnP.Core.Services.Builder.Configuration;

namespace Mcd79.MigrationValidationEngine.Core.Test.Services
{
    public class PopulateTenantInfoTests
    {
        IHost _host;
        private readonly IConfiguration _config;

        public PopulateTenantInfoTests() {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"appsettings.json",false,false)
                .AddEnvironmentVariables()
                .Build();

            _host = Host.CreateDefaultBuilder()
            // Configure logging
            .ConfigureServices((hostingContext, services) =>
            {
                // Add the PnP Core SDK library services
                services.AddPnPCore();
                // Add the PnP Core SDK library services configuration from the appsettings.json file
                services.Configure<PnPCoreOptions>(hostingContext.Configuration.GetSection("PnPCore"));
                // Add the PnP Core SDK Authentication Providers
                services.AddPnPCoreAuthentication();
                // Add the PnP Core SDK Authentication Providers configuration from the appsettings.json file
                services.Configure<PnPCoreAuthenticationOptions>(hostingContext.Configuration.GetSection("PnPCore"));
            })
            // Let the builder know we're running in a console
            .UseConsoleLifetime()
            // Add services to the container
            .Build();

            Console.WriteLine(_host);
        }
        
        public Task InitializeAsync() => _host.StartAsync();

        [Fact]
        public async Task Test1()
        {
            
            var siteUrl = "https://mcdonnell.sharepoint.com/sites/MVETest";
            using (var scope = _host.Services.CreateScope())
            {
                // Ask an IPnPContextFactory from the host
                var pnpContextFactory = scope.ServiceProvider.GetRequiredService<IPnPContextFactory>();

                // Create a PnPContext
                using (var context = await pnpContextFactory.CreateAsync(new Uri(siteUrl)))
                {
                    // Load the Title property of the site's root web
                    await context.Web.LoadAsync(p => p.Title);
                    Console.WriteLine($"The title of the web is {context.Web.Title}");
                }
            }
        }
    }
}
