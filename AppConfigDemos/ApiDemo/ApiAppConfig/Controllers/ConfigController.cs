using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using ApiAppConfig.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiAppConfig.Controllers
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
