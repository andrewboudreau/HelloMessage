using System;
using System.Threading;
using System.Threading.Tasks;

using AzureFunctionHost.Infrastructure;
using MediatR;

namespace AzureFunctionHost.Application.Approvals
{
    public class ApproveSubmission : IRequest
    {
        public ApproveSubmission(string userId, params Guid[] submissions)
        {
            ApproverId = userId;
            Submissions = submissions;
        }

        public string ApproverId { get; private set; }

        public Guid[] Submissions { get; private set; }
    }

    public class ApproveSubmissionsHandler : IRequestHandler<ApproveSubmission>
    {
        private readonly SubmissionRepository submissions;
        private readonly IPublisher publisher;

        public ApproveSubmissionsHandler(SubmissionRepository submissions, IPublisher publisher)
        {
            this.submissions = submissions;
            this.publisher = publisher;
        }

        public Task<Unit> Handle(ApproveSubmission request, CancellationToken cancellationToken)
        {
            foreach (var submissionId in request.Submissions)
            {
                var submission = submissions.Find(submissionId);
                submission.ApproveBy(request.ApproverId);
                publisher.PublishAllDomainEvents(submission);
            }

            return Unit.Task;
        }
    }
}
