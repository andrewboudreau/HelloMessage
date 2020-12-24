using System;
using System.Threading;
using System.Threading.Tasks;
using AzureFunctionHost.Domain;
using AzureFunctionHost.Infrastructure;
using MediatR;

namespace AzureFunctionHost.Application
{
    public class SubmitForApproval : IRequest<Guid>
    {
        public SubmitForApproval(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }

    public class SubmitForApprovalHandler : IRequestHandler<SubmitForApproval, Guid>
    {
        private readonly SubmissionRepository submissions;

        public SubmitForApprovalHandler(SubmissionRepository submissions)
        {
            this.submissions = submissions;
        }

        public Task<Guid> Handle(SubmitForApproval request, CancellationToken cancellationToken)
        {
            var submission = Submission.For(request.UserId);
            submissions.Add(submission);

            return Task.FromResult(submission.SubmissionId);
        }
    }
}
