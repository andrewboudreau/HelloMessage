using System;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctionHost.Application;
using AzureFunctionHost.Application.Submissions;
using AzureFunctionHost.Domain;
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

        [FunctionName("Submit-Send")]
        public async Task<IActionResult> Submit([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "submit/{user}")] HttpRequest _, string user, ILogger log)
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

        [FunctionName("Submit-Query")]
        public async Task<IActionResult> Query(
             [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "submit/{submissionId:guid}")] HttpRequest _, Guid submissionId)
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
