using FunctionAppConfigPollingRefresh.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Options;

namespace FunctionAppConfigPollingRefresh
{
    public class ConfigFunction
    {
        private readonly Settings settings;

        public ConfigFunction(IOptionsSnapshot<Settings> options)
        {
            this.settings = options.Value;
        }

        [FunctionName("ConfigFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            return new OkObjectResult(settings);
        }
    }
}
