using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;
using System.Threading.Tasks;

namespace AppConfigConsoleWithKeyVaultDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = null;
            IConfigurationRefresher refresher = null;

            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                        new KeyVaultClient
                        .AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(options =>
            {
                options
                    .ConnectWithManagedIdentity("https://abc1234configstore.azconfig.io")
                    .Use("MyApp1:*")
                    .ConfigureRefresh(refresh =>
                    {
                        refresh
                        .Register("MyApp1:Sentinel", refreshAll: true)
                        .SetCacheExpiration(TimeSpan.FromSeconds(1));
                    }).UseAzureKeyVault(keyVaultClient);

                refresher = options.GetRefresher();
            });

            configuration = builder.Build();
            PrintConfig(configuration);
            refresher.Refresh().Wait();
            PrintConfig(configuration);


        }

        static void PrintConfig(IConfiguration configuration)
        {
            Console.WriteLine("_______________________________________________");
            foreach (var configItem in configuration.AsEnumerable())
            {
                Console.WriteLine($"Key {configItem.Key} -> Value {configItem.Value}");
            }
            Console.WriteLine();
        }
    }
}
