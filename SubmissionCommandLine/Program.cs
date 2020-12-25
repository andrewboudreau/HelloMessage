using System;
using System.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SubmissionCommandLine
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var done = false;

            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.Configure<SubmissionClientOptions>(context.Configuration.GetSection(SubmissionClientOptions.SubmissionClient));
                    services.AddHttpClient<>("submission", client => 
                    {
                        client.BaseAddress = context
                    });

                })
                .ConfigureLogging(builder => builder.AddConsole().AddDebug())
                .RunConsoleAsync();


            var options = new SubmissionClientOptions().WithBaseAddress(new Uri("http://localhost:7071/api/"));
            using var client = SubmissionClientFactory(options);

            // entry to run app
            //await serviceProvider.GetService<App>().Run(args);

            Console.WriteLine("Shutting Down");

            async Task submitter()
            {
                Guid submissionId = default;

                do
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter a name for the submission:");
                    var username = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(username))
                    {
                        continue;
                    }
                    else if (username.ToLowerInvariant() == "c" || username.ToLowerInvariant() == "check" || username.ToLowerInvariant() == "status")
                    {
                        if (submissionId == default)
                        {
                            Console.WriteLine($"First send a submission by entering a name.");
                            continue;
                        }

                        Console.WriteLine($"Checking submission status of {submissionId}");
                        var url = options.Submit.Replace("{user}", username);
                        var response = await client.PostAsync(url, null);

                        if (!response.IsSuccessStatusCode)
                        {
                            done = true;
                            Console.WriteLine(await response.Content.ReadAsStringAsync());
                            response.EnsureSuccessStatusCode();
                        }
                        else
                        {
                            submissionId = await JsonSerializer.DeserializeAsync<Guid>(await response.Content.ReadAsStreamAsync());
                            Console.WriteLine($"Your submission has been accepted. SubmissionId: {submissionId}");
                        }
                    }
                    else if (username.ToLowerInvariant() == "q" || username.ToLowerInvariant() == "quit")
                    {
                        done = true;
                    }
                    else
                    {
                        Console.WriteLine($"Sending submission for {username}");
                        var url = options.Submit.Replace("{user}", username);
                        var response = await client.PostAsync(url, null);

                        if (!response.IsSuccessStatusCode)
                        {
                            done = true;
                            Console.WriteLine(await response.Content.ReadAsStringAsync());
                            response.EnsureSuccessStatusCode();
                        }
                    }

                } while (!done);
            }

            await Task.Run(submitter);
        }

        private static HttpClient SubmissionClientFactory(SubmissionClientOptions options)
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = options.BaseAddress
            };

            httpClient.DefaultRequestHeaders.Add("X-AccessToken", options.AccessToken);

            return httpClient;
        }
    }
}
