using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SubmissionCommandLine
{
    internal sealed class CommandLineTerminal : IHostedService
    {
        private readonly ILogger logger;
        private readonly IHostApplicationLifetime appLifetime;

        public CommandLineTerminal(ILogger<CommandLineTerminal> logger, IHostApplicationLifetime appLifetime)
        {
            this.logger = logger;
            this.appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {

                    try
                    {

                        // Simulate real work is being done
                        await Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Unhandled exception");
                    }
                    finally
                    {
                        // Stop the application once the work is done
                        appLifetime.StopApplication();
                    }
                });
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
