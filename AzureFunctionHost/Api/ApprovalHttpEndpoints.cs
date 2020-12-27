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
        private readonly ISender mediator;

        public ApprovalHttpEndpoints(ISender mediator)
        {
            this.mediator = mediator;
        }

        [FunctionName("GetPending")]
        public async Task<IActionResult> GetPending([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "approval/pending")] HttpRequest httpRequest)
        {
            var query = new QueryPending();
            var results = await mediator.Send(query);

            return new OkObjectResult(results);
        }

        [FunctionName("PostApproval")]
        public async Task<IActionResult> PostApproval(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "approve/{submissionId}/by/{approverId}")] HttpRequest httpRequest, string user)
        {
            var request = new ApproveAllSubmissions(user);
            await mediator.Send(request);

            return new OkResult();
        }
    }
}
