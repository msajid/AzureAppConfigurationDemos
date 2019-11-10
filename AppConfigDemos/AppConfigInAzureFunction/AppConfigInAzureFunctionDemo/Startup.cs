using System;
using AppConfigInAzureFunctionDemo.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(AppConfigInAzureFunctionDemo.Startup))]

namespace AppConfigInAzureFunctionDemo
{
    public class Startup : FunctionsStartup
    {  
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfigurationRoot configurationRoot = null;
            IConfigurationRefresher configurationRefresher = null;

            var tempBuilder = new ConfigurationBuilder();

            tempBuilder.AddAzureAppConfiguration(options =>
            {
                options
                .ConnectWithManagedIdentity("https://abc1234configstore.azconfig.io")
                .ConfigureRefresh(refresh =>
                 {
                     refresh
                     .Register("MyApp1:Sentinel", true)
                     .SetCacheExpiration(TimeSpan.FromSeconds(10));
                 });
                configurationRefresher = options.GetRefresher();

            });
            configurationRoot = tempBuilder.Build();

            builder.Services.Configure<Settings>(configurationRoot.GetSection("MyApp1"));
            builder.Services.AddSingleton(configurationRefresher);

        }
    }
}

