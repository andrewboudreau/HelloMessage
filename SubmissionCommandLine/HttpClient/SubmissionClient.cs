using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SubmissionCommandLine
{
    public interface ISubmissionClient
    {
        Task<Guid> PostSubmission(string userId);

        Task<bool> GetStatus(Guid submissionId);
    }

    public class SubmissionClient : ISubmissionClient
    {
        private readonly IOptions<SubmissionClientOptions> options;
        private readonly HttpClient httpClient;

        public SubmissionClient(IOptions<SubmissionClientOptions> options, HttpClient httpClient)
        {
            this.options = options;
            this.httpClient = httpClient;
        }

        public async Task<bool> GetStatus(Guid submissionId)
        {
            using var response = await httpClient.GetAsync(options.Value.Status.Replace("{submissionId}", submissionId.ToString()));
            return await JsonSerializer.DeserializeAsync<bool>(await response.Content.ReadAsStreamAsync());
        }

        public async Task<Guid> PostSubmission(string userId)
        {
            using var response = await httpClient.PostAsync(options.Value.Submit.Replace("{userId}", userId), null);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                response.EnsureSuccessStatusCode();
            }

            var submissionId = await JsonSerializer.DeserializeAsync<Guid>(await response.Content.ReadAsStreamAsync());
            return submissionId;
        }
    }
}
