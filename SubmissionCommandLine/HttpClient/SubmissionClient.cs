using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SubmissionCommandLine
{
    /// <summary>
    /// Contracts for sending and checking Submissions.
    /// </summary>
    public interface ISubmissionClient
    {
        Task<Guid> PostSubmission(string userId);

        Task<SubmissionStatus> GetStatus(Guid submissionId);
    }

    public enum SubmissionStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    /// <summary>
    /// An HTTP client for sending and checking submissions.
    /// </summary>
    public class SubmissionClient : ISubmissionClient
    {
        private readonly IOptions<SubmissionClientOptions> options;
        private readonly HttpClient httpClient;

        public SubmissionClient(IOptions<SubmissionClientOptions> options, HttpClient httpClient)
        {
            this.options = options;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Posts a Submission.
        /// </summary>
        /// <param name="userId">The user who is posting the submission.</param>
        /// <returns>A Guid which can be used to check the submission status.</returns>
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

        /// <summary>
        /// Gets true if the submission has been approved or false otherwise.
        /// </summary>
        /// <param name="submissionId">The submission to check.</param>
        /// <returns>True if the submission has been approved.</returns>
        public async Task<SubmissionStatus> GetStatus(Guid submissionId)
        {
            using var response = await httpClient.GetAsync(options.Value.Status.Replace("{submissionId}", submissionId.ToString()));
            var status = await response.Content.ReadAsStringAsync();

            return status.ToLowerInvariant() switch
            {
                "pending" => SubmissionStatus.Pending,
                "approved" => SubmissionStatus.Approved,
                "rejected" => SubmissionStatus.Rejected,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
