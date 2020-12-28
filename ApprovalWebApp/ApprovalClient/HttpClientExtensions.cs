using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ApprovalWebApp
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClient<TClient, TImplementation, TClientOptions>(
           this IServiceCollection services,
           IConfiguration configuration)
           where TClient : class
           where TImplementation : class, TClient
           where TClientOptions : HttpClientOptions, new() =>
           services
               .Configure<TClientOptions>(configuration.GetSection(typeof(TClientOptions).Name[..^7]))
               //.AddTransient<CorrelationIdDelegatingHandler>()
               //.AddTransient<UserAgentDelegatingHandler>()
               .AddHttpClient<TClient, TImplementation>()
               .ConfigureHttpClient((sp, options) =>
               {
                   var httpClientOptions = sp
                       .GetRequiredService<IOptions<TClientOptions>>()
                       .Value;
                   options.BaseAddress = httpClientOptions.BaseAddress;
                   // options.Timeout = httpClientOptions.Timeout;
               })
               //.ConfigurePrimaryHttpMessageHandler(x => new DefaultHttpClientHandler())
               //.AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
               //.AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker)
               //.AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
               //.AddHttpMessageHandler<UserAgentDelegatingHandler>()
               .Services;
    }
}
