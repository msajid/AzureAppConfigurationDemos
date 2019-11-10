using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using AppConfigInAzureFunctionDemo.Models;
using Microsoft.Extensions.Options;

namespace AppConfigInAzureFunctionDemo
{
    public class SettingsFunction
    {
        private readonly Settings settings;

        public SettingsFunction(IOptionsSnapshot<Settings> options)
        {
            this.settings = options.Value;
        }

        [FunctionName("Settings")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
           
            return new OkObjectResult(settings);

        }
    }
}
