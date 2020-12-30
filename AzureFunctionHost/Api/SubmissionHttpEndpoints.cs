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
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;

namespace AzureFunctionHost.Api
{
    public class SubmissionHttpEndpoints
    {
        private readonly ISender mediator;

        public SubmissionHttpEndpoints(ISender mediator)
        {
            this.mediator = mediator;
        }

        [FunctionName("PostSubmission")]
        public async Task<IActionResult> PostSubmission(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "submission/{user}")] HttpRequest httpRequest,
            string user,
            [SignalR(HubName = "SubmissionHub")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            log.LogInformation($"Request Submitted for '{user}'.");
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentException("Cannot be null or empty.", nameof(user));
            }

            var request = new SubmitForApproval(user);
            var response = await mediator.Send(request);

            var message = new SignalRMessage
            {
                Target = $"broadcastMessage",
                Arguments = new[] { request.UserId, "submitted" }
            };

            await signalRMessages.AddAsync(message);

            return new OkObjectResult(response);
        }

        [FunctionName("GetStatus")]
        public async Task<IActionResult> GetStatus(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "submission/{submissionId:guid}")] HttpRequest httpRequest,
            Guid submissionId,
            [SignalR(HubName = "SubmissionHub")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            if (submissionId == default)
            {
                throw new ArgumentException("Cannot be null, empty, or default.", nameof(submissionId));
            }

            var query = new QueryStatus(submissionId);
            var status = await mediator.Send(query);

            return new OkObjectResult(status.ToString());
        }
    }
}
