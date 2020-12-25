using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionCommandLine
{
    public interface ISubmissionClient
    {
        Task<bool> GetStatus(Guid submissionId);
    }

    public class SubmissionClient : ISubmissionClient
    {
        private readonly HttpClient httpClient;

        public SubmissionClient(HttpClient httpClient) => this.httpClient = httpClient;

        public async Task<bool> GetStatus(Guid submissionId)
        {
            var response = await httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
            return null;// await response.Content.ReadAsAsync<Guid>();
        }
    }
}
