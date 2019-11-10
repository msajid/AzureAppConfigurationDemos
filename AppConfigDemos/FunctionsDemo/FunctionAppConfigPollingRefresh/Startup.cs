using FunctionAppConfigPollingRefresh.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FunctionAppConfigPollingRefresh.Startup))]

namespace FunctionAppConfigPollingRefresh
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // using configuration builder to load the json settings to get the App Config endpoint
            var tempConfig =
                new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .Build();

            IConfigurationRefresher configurationRefresher = null;

            var configurationRoot =
                new ConfigurationBuilder()
                 .SetBasePath(Environment.CurrentDirectory)
                 .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .AddAzureAppConfiguration(options =>
                 {
                     options
                      .ConnectWithManagedIdentity(tempConfig["AppConfigurationEndpoint"])
                      .ConfigureRefresh(refresh =>
                      {
                          refresh
                            .Register("MyApp1:Sentinel", true);
                      });
                     configurationRefresher = options.GetRefresher();

                 })
                 .Build();

            builder.Services.Configure<Settings>(configurationRoot.GetSection("MyApp1"));
            builder.Services.AddSingleton(configurationRefresher);
            // optionally add the IConfiguration
            builder.Services.AddSingleton<IConfiguration>(configurationRoot);

        }
    }
}

