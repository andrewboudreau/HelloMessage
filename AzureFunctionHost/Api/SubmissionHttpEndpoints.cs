using System;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctionHost.Application;
using AzureFunctionHost.Application.Submissions;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionHost.Api
{
    public class SubmissionHttpEndpoints
    {
        private readonly ISender sender;

        public SubmissionHttpEndpoints(ISender sender)
        {
            this.sender = sender;
        }

        [FunctionName("Submission")]
        public async Task<IActionResult> Submit([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "submission/{user}")] HttpRequest httpRequest, string user, ILogger log)
        {
            log.LogInformation($"Request Submitted for '{user}'.");
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentException("Cannot be null or empty.", nameof(user));
            }

            var request = new SubmitForApproval(user);
            var response = await sender.Send(request);

            return new OkObjectResult(response);
        }

        [FunctionName("SubmissionStatus")]
        public async Task<IActionResult> Query(
             [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "submission/{submissionId:guid}")] HttpRequest httpRequest, Guid submissionId)
        {
            if (submissionId == default)
            {
                throw new ArgumentException("Cannot be null, empty, or default.", nameof(submissionId));
            }

            var query = new QuerySubmissionStatus(submissionId);
            return new OkObjectResult(await sender.Send(query));
        }
    }
}
