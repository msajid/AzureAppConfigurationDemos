using System;
using Azure.Data.AppConfiguration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AppConfigInAzureFunctionReplicator
{
    public static class Syncer
    {
        [FunctionName("Syncer")]
        public static void Run([ServiceBusTrigger("abc123q", Connection = "ConnectionString")]string myQueueItem, ILogger log)
        {
            string primaryConnection = "<primary connection>";
            var primary = new ConfigurationClient(primaryConnection);

            string secondaryConnection = "<secondary connection>";
            var secondary = new ConfigurationClient(secondaryConnection);


            var message = primary.GetConfigurationSetting("MyApp1:Message");
            var response = primary.SetConfigurationSetting("MyApp1:Message",
                DateTime.Now.ToString(), "Development");
            message = primary.GetConfigurationSetting("MyApp1:Message", "Development");

            ReplicateConfiguration(primary, secondary);
        }

        static void ReplicateConfiguration(ConfigurationClient primary, ConfigurationClient secondary)
        {
            var settingSelector = new SettingSelector();
            var allsettings = primary.GetConfigurationSettings(settingSelector);
            foreach (var currentSetting in allsettings)
            {
                secondary.SetConfigurationSetting(currentSetting);
            }
        }
    }
}
