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
            Console.WriteLine($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        bool quit = false;
                        Guid submissionId = default;

                        do
                        {
                            var username = WaitForUserInput();
                            if (string.IsNullOrWhiteSpace(username))
                            {
                                continue;
                            }
                            else if (username.Equals("q", StringComparison.OrdinalIgnoreCase))
                            {
                                quit = true;
                            }
                            else if (username.Equals("c", StringComparison.OrdinalIgnoreCase))
                            {
                                if (submissionId == default)
                                {
                                    Console.WriteLine($"First send a submission by entering a name.");
                                    continue;
                                }

                                var status = await client.GetStatus(submissionId);
                                logger.LogInformation($"Status is {status} for {submissionId}.");
                            }
                            else
                            {
                                submissionId = await client.PostSubmission(username);
                                logger.LogInformation($"Submission has been accepted. SubmissionId: {submissionId}");
                            }
                        } while (!quit);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Unhandled exception");
                        exitCode = 1;
                    }
                    finally
                    {
                        appLifetime.StopApplication();
                    }
                });
            });

            return Task.CompletedTask;
        }

        private string WaitForUserInput()
        {
            Console.WriteLine();
            Console.Write("Enter a name for the submission:");
            return Console.ReadLine();
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
