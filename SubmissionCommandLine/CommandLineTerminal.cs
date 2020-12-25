using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SubmissionCommandLine
{
    internal sealed class CommandLineTerminal : IHostedService
    {
        private readonly ISubmissionClient client;
        private readonly ILogger logger;
        private readonly IHostApplicationLifetime appLifetime;

        private int? exitCode;

        public CommandLineTerminal(ISubmissionClient client, ILogger<CommandLineTerminal> logger, IHostApplicationLifetime appLifetime)
        {
            this.client = client;
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
                        var submissionId = client.PostSubmission("andrewboudreau@gmail.com");
                        logger.LogInformation($"Submission accepted {submissionId}");                        // Simulate real work is being done
                        await Task.Delay(3000);
                        logger.LogInformation($"Done with fake wait...");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Unhandled exception"); 
                        exitCode = 1;
                    }
                    finally
                    {
                        logger.LogInformation($"Finally block");
                        // Stop the application once the work is done
                        appLifetime.StopApplication();
                        logger.LogInformation($"After stop");
                    }
                });
            });

            logger.LogInformation($"returning task");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug($"Exiting with return code: {exitCode}");

            // Exit code may be null if the user canceled via CTRL+C/SIGTERM
            Environment.ExitCode = exitCode.GetValueOrDefault(-1);
            return Task.CompletedTask;
        }
    }
}
