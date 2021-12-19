using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using PnP.Core.Auth;
using PnP.Core.Auth.Services.Builder.Configuration;
using PnP.Core.Services;
using PnP.Core.Services.Builder;
using PnP.Core.Services.Builder.Configuration;
using Mcd79.MigrationValidationEngine.Core.Model;

namespace Mcd79.MigrationValidationEngine.Core.Services
{
    public static class Core
    {
        public static IHost RetrieveHost () {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"appsettings.json", false, false)
                .AddEnvironmentVariables()
                .Build();

            IHost host = Host.CreateDefaultBuilder()
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

            return host;
        }

        public static IServiceScope RetrieveScope(IHost host) {
            return host.Services.CreateScope();
        }

        public static Task<PnPContext> RetrieveContext(string SiteUrl, IServiceScope Scope) {
            try
            {
                // Ask an IPnPContextFactory from the host
                    var pnpContextFactory = Scope.ServiceProvider.GetRequiredService<IPnPContextFactory>();

                    // Create a PnPContext
                    return pnpContextFactory.CreateAsync(new Uri(SiteUrl));
                    }
            catch (MsalClientException msalExp)
            {
                Console.WriteLine("Damn, msal error: " + msalExp.Message);
                throw msalExp;
            }
            catch(Exception exp)
            {
                Console.WriteLine("Damn, random error: " + exp.Message);
                throw exp;
            }
        }
    }
}
