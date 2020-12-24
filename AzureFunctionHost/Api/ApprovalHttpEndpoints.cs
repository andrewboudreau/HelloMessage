using System.Linq;
using AzureFunctionHost.Application.Approvals;
using AzureFunctionHost.Domain;
using AzureFunctionHost.Infrastructure;
using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionHost.Application
{
    public class ApprovalHttpEndpoints
    {
        private readonly ISender sender;

        public ApprovalHttpEndpoints(ISender sender)
        {
            this.sender = sender;
        }

        [FunctionName("Approve")]
        public IActionResult Approve(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "approval/all/{user}")] HttpRequest _, string user)
        {
            sender.Send(new ApproveAllSubmissions(user));
            return new OkResult();
        }

        [FunctionName("ApproveQuery")]
        public IActionResult Query([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "approval/pending")] HttpRequest _)
        {
            return new OkObjectResult(sender.Send(new QueryPendingSubmissions()));
        }
    }
}
