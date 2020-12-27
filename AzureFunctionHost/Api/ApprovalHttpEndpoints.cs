using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

using AzureFunctionHost.Application.Approvals;

namespace AzureFunctionHost.Application
{
    public class ApprovalHttpEndpoints
    {
        private readonly ISender sender;

        public ApprovalHttpEndpoints(ISender sender)
        {
            this.sender = sender;
        }

        [FunctionName("Approval")]
        public async Task<IActionResult> Approve(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "approval/all/{user}")] HttpRequest httpRequest, string user)
        {
            var request = new ApproveAllSubmissions(user);
            await sender.Send(request);

            return new OkResult();
        }

        [FunctionName("ApprovalBatch")]
        public async Task<IActionResult> ApproveBatch(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "approval/all/{user}")] HttpRequest httpRequest, string user)
        {
            var request = new ApproveAllSubmissions(user);
            await sender.Send(request);

            return new OkResult();
        }

        [FunctionName("ApprovalQuery")]
        public async Task<IActionResult> Query([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "approval/pending")] HttpRequest httpRequest)
        {
            var query = new QueryPendingSubmissions();
            var results = await sender.Send(query);

            return new OkObjectResult(results);
        }
    }
}
