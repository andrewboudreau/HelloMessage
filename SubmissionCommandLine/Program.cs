using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SubmissionCommandLine
{
    class Program
    {
        static async Task Main(string[] args) =>
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                    services
                        .AddHttpClient<ISubmissionClient, SubmissionClient, SubmissionClientOptions>(context.Configuration)
                        .AddHostedService<CommandLineTerminal>())
                .ConfigureLogging(loggingBuilder => loggingBuilder.AddConsole().AddDebug())
                .RunConsoleAsync();
    }
}
