using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace AppConfigInAspnetcore
{

    class MyAppConfigurationRefreshMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfigurationRefresher _refresher;

        public MyAppConfigurationRefreshMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _refresher = ((IConfigurationRoot)configuration).Providers.FirstOrDefault(p => p is IConfigurationRefresher) as IConfigurationRefresher;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_refresher != null)
            {

                await _refresher.Refresh();

            }
            await _next(context);
        }
    }

}
