using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
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
using Mcd79.MigrationValidationEngine.Core.Services;

namespace Mcd79.MigrationValidationEngine.Core.Test.Services
{
    public class PopulateTenantInfoTests
    {
        IHost _host;
        private readonly IConfiguration _config;
        private DataRepo _dataRepo;

        public PopulateTenantInfoTests()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@"appsettings.json", false, false)
                .AddEnvironmentVariables()
                .Build();
            _host = Core.Services.Core.RetrieveHost();
            var section = _config.GetSection(nameof(DataRepoConfig));
            var dataRepoConfig = section.Get<DataRepoConfig>();

            _dataRepo = new DataRepo(dataRepoConfig);
        }

        public async Task InitializeAsync() => _host.StartAsync();
        
        [Fact]
        public async Task PopulateTenantInfoTest()
        {
            var siteUrl = "https://mcdonnell.sharepoint.com/sites/MVETest";
            try
            {
                using (var scope = _host.Services.CreateScope())
                {
                    // Create a PnPContext
                    using (var context = await Core.Services.Core.RetrieveContext(siteUrl, scope))
                    {
                        // Load the Title property of the site's root web
                        await context.Web.LoadAsync(p => p.Title);
                        MVETenant tenant = new MVETenant();
                        tenant.TenantAlias = context.Site.Url.ToString();
                        _dataRepo.Save<MVETenant>(tenant);
                        Console.WriteLine($"The title of the web is {context.Web.Title}");
                    }
                }

            }
            catch (MsalClientException msalExp)
            {
                Console.WriteLine("Damn, msal error: " + msalExp.Message);
            }
            catch(Exception exp)
            {
                Console.WriteLine("Damn, random error: " + exp.Message);
            }
        }
    }
}
