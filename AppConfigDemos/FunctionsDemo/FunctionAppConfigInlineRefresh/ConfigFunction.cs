using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace FunctionAppConfigInlineRefresh
{
    public class ConfigFunction
    {
        private readonly IConfigurationRefresher configurationRefresher;
        private readonly IConfiguration configuration;

        public ConfigFunction(IConfigurationRefresher configurationRefresher, IConfiguration configuration)
        {
            this.configurationRefresher = configurationRefresher;
            this.configuration = configuration;
        }

        [FunctionName("ConfigFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            await configurationRefresher.Refresh();
            var myApp1Config = configuration.GetSection("MyApp1");
            return new OkObjectResult(myApp1Config["Message"]);

        }
    }
}
