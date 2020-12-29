using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ApprovalWebApp
{
    public interface IApprovalClient
    {
        Task PostApproval(string approverId, Guid submissionId);

        Task PostRejection(string approverId, Guid submissionId);

        Task PostApprovalAll(string approverId);

        Task<PendingSubmission[]> GetPending();

        Task<SubmissionAudit[]> GetAudits(int skip, int take);
    }

    public class ApprovalClient : IApprovalClient
    {
        private readonly static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        private readonly IOptions<ApprovalClientOptions> options;

        private readonly HttpClient httpClient;

        public ApprovalClient(IOptions<ApprovalClientOptions> options, HttpClient httpClient)
        {
            this.options = options;
            this.httpClient = httpClient;
        }

        public async Task<PendingSubmission[]> GetPending()
        {
            using var response = await httpClient.GetAsync(options.Value.GetPending);
            return await Deserialize<PendingSubmission[]>(await response.Content.ReadAsStreamAsync());
        }

        public async Task<SubmissionAudit[]> GetAudits(int skip, int take)
        {
            using var response = await httpClient.GetAsync(options.Value.GetAudits);
            Console.WriteLine("GetAudits: " + await response.Content.ReadAsStringAsync());
            return await Deserialize<SubmissionAudit[]>(await response.Content.ReadAsStreamAsync());
        }

        public async Task PostApproval(string approverId, Guid submissionId)
        {
            var url = options.Value.PostApproval
                .Replace("{approverId}", approverId)
                .Replace("{submissionId}", submissionId.ToString());

            using var response = await httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task PostRejection(string approverId, Guid submissionId)
        {
            var url = options.Value.PostRejection
                .Replace("{approverId}", approverId)
                .Replace("{submissionId}", submissionId.ToString());

            using var response = await httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                response.EnsureSuccessStatusCode();
            }

        }

        public async Task PostApprovalAll(string approverId)
        {
            var url = options.Value.PostApprovalAll
                .Replace("{approverId}", approverId);

            using var response = await httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                response.EnsureSuccessStatusCode();
            }
        }

        private static async Task<T> Deserialize<T>(Stream stream)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, options: jsonSerializerOptions);
        }
    }
}
