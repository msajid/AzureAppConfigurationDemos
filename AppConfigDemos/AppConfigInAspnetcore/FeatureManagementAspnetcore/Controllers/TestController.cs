using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureManagementAspnetcore.Controllers
{
    public enum MyFeatureFlags
    {
        Beta
    }

    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IFeatureManager _featureManager;

        public TestController(IFeatureManagerSnapshot featureManager)
        {
            _featureManager = featureManager;
        }

        [FeatureGate(MyFeatureFlags.Beta)]
        [HttpGet("future")]
        public IActionResult GetFuture()
        {
            return Ok("future is bright");
        }

        [HttpGet("current")]
        public IActionResult GetCurrent()
        {
            return Ok("present is great");
        }
    }
}
