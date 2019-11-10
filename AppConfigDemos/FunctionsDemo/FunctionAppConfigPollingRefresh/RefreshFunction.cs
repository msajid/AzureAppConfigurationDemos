using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using System;

namespace FunctionAppConfigPollingRefresh
{
    public class RefreshFunction
    {
        private readonly IConfigurationRefresher configurationRefresher;

        public RefreshFunction(IConfigurationRefresher configurationRefresher)
        {
            this.configurationRefresher = configurationRefresher;
        }

        [FunctionName("RefreshFunction")]
        public void Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer)
        {
            try
            {
                configurationRefresher.Refresh();
            }
            catch (KeyVaultReferenceException)
            {
                // Keyvalue reference error. log exception and/or throw. Propagate as appropriate.
            }
            catch (Exception)
            {
                // log exception and / or throw.
                // Ignore unknown errors for now (not the best approach).
            }

        }
    }
}
