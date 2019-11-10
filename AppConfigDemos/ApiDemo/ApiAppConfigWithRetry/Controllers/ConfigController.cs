using ApiAppConfigWithRetry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ApiAppConfigWithRetry.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private Settings settings;

        public ConfigController(IOptionsSnapshot<Settings> options)
        {
            this.settings = options.Value;
        }

        [HttpGet]
        public Settings Get()
        {
            return settings;
        }
    }
}
