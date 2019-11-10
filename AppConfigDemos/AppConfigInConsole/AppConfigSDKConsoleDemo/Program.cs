using Azure.Data.AppConfiguration;
using System;

namespace AppConfigSDKConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
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
