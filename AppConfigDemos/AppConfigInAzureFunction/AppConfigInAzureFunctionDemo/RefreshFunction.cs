using System;
using AppConfigInAzureFunctionDemo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppConfigInAzureFunctionDemo
{
    public class RefreshFunction
    {
        private readonly IConfigurationRefresher configurationRefresher;
      
        public RefreshFunction(IConfigurationRefresher configurationRefresher)
        {
            this.configurationRefresher = configurationRefresher;
        }

        [FunctionName("RefreshFunction")]
        public void Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            configurationRefresher.Refresh();

        }
    }
}
