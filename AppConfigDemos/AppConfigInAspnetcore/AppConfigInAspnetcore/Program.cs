using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppConfigInAspnetcore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) => 
                {
                    config.AddAzureAppConfiguration(options => 
                    {
                        options
                        .ConnectWithManagedIdentity("https://abc1234configstore.azconfig.io")
                        .Use("MyApp1:*", "Development")
                        .ConfigureRefresh(refresh =>
                        {
                            refresh
                            .Register("MyApp1:Sentinel", true)
                            .SetCacheExpiration(TimeSpan.FromSeconds(10));
                        });
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
