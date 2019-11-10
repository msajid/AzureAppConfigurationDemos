using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;
using System.Threading.Tasks;

namespace AppConfigConsoleDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfigurationRefresher refresher = null;
            IConfiguration configuration = null;
         
            var builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(options =>
            {
                options
                .ConnectWithManagedIdentity("https://abc1234configstore.azconfig.io")
                .ConfigureRefresh(refresh =>
                {
                    refresh
                    .Register("test", refreshAll: true)
                    .SetCacheExpiration(TimeSpan.FromSeconds(1));

                });
                refresher = options.GetRefresher();
            });

            configuration = builder.Build();

            PrintConfig(configuration);
            await refresher.Refresh();
            PrintConfig(configuration);

        }

        static void PrintConfig(IConfiguration configuration)
        {
            Console.WriteLine("_______________________________________________");
            foreach (var configItem in configuration.AsEnumerable())
            {
                Console.WriteLine($"Key {configItem.Key} -> Value {configItem.Value}");
            }
            

        }
    }
}
