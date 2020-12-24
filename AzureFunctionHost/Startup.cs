using System.Reflection;
using AzureFunctionHost.Infrastructure;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureFunctionHost.Startup))]

namespace AzureFunctionHost
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(new ApprovalRepository());
            builder.Services.AddSingleton(new SubmissionRepository());

            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}

