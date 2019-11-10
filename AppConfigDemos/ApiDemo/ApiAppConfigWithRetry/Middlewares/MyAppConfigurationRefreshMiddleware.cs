using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Polly;

namespace ApiAppConfigWithRetry
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
                    var policy = Policy
                                   .Handle<KeyVaultReferenceException>()
                                   .Or<Exception>()
                                   .WaitAndRetryAsync(3, s =>
                                   {
                                       return TimeSpan.FromSeconds(2*s);
                                   }, 
                                   onRetry: (response, delay, retryCount, context) =>
                                   {
                                       // log on retry if required.
                                       
                                   });

                    await policy.ExecuteAsync(async () => 
                    {
                        await _refresher.Refresh();
                    });
                                                           
                }
                await _next(context);
            }
        }
    
}
