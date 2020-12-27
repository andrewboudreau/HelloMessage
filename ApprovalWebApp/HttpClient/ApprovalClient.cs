using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ApprovalWebApp
{
    public interface IApprovalClient
    {
        Task<Guid> PostApproval(string userId, Guid submissionId);

        Task<IEnumerable<Guid>> GetPending();
    }

    public class ApprovalClient : IApprovalClient
    {
        private readonly IOptions<ApprovalClientOptions> options;
        private readonly HttpClient httpClient;

        public ApprovalClient(IOptions<ApprovalClientOptions> options, HttpClient httpClient)
        {
            this.options = options;
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Guid>> GetPending()
        {
            using var response = await httpClient.GetAsync(options.Value.GetPending);
            return await JsonSerializer.DeserializeAsync<IEnumerable<Guid>>(await response.Content.ReadAsStreamAsync());
        }

        public async Task<Guid> PostApproval(string userId, Guid submissionId)
        {
            var url = options.Value.PostApproval
                .Replace("{userId}", userId)
                .Replace("{submissionId}", submissionId.ToString());

            using var response = await httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                response.EnsureSuccessStatusCode();
            }

            var approvalId = await JsonSerializer.DeserializeAsync<Guid>(await response.Content.ReadAsStreamAsync());
            return approvalId;
        }
    }
}
