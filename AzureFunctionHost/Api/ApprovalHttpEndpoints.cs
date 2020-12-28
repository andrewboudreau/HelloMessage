using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

using AzureFunctionHost.Application.Approvals;
using System;

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
        public async Task<IActionResult> GetPending([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "approve")] HttpRequest httpRequest)
        {
            var query = new QueryPending();
            var results = await mediator.Send(query);

            return new OkObjectResult(results);
        }

        [FunctionName("PostApproval")]
        public async Task<IActionResult> PostApproval(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "approve/{submissionId:guid}/by/{approverId}")] HttpRequest httpRequest, 
            Guid submissionId, 
            string approverId)
        {
            var request = new ApproveAllSubmissions(approverId);
            await mediator.Send(request);

            return new OkResult();
        }

        [FunctionName("PostApprovalAll")]
        public async Task<IActionResult> PostApprovalAll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "approve/all/by/{approverId}")] HttpRequest httpRequest,
            string approverId)
        {
            var request = new ApproveAllSubmissions(approverId);
            await mediator.Send(request);

            return new OkResult();
        }
    }
}
