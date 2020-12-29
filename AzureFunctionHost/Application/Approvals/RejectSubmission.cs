using System;
using System.Threading;
using System.Threading.Tasks;

using AzureFunctionHost.Infrastructure;
using MediatR;

namespace AzureFunctionHost.Application.Approvals
{
    public class RejectSubmission : IRequest
    {
        public RejectSubmission(string userId, params Guid[] submissions)
        {
            ApproverId = userId;
            Submissions = submissions;
        }

        public string ApproverId { get; private set; }

        public Guid[] Submissions { get; private set; }
    }

    public class RejectSubmissionHandler : IRequestHandler<RejectSubmission>
    {
        private readonly SubmissionRepository submissions;
        private readonly IPublisher publisher;

        public RejectSubmissionHandler(SubmissionRepository submissions, IPublisher publisher)
        {
            this.submissions = submissions;
            this.publisher = publisher;
        }

        public Task<Unit> Handle(RejectSubmission request, CancellationToken cancellationToken)
        {
            foreach (var submissionId in request.Submissions)
            {
                var submission = submissions.Find(submissionId);
                submission.RejectBy(request.ApproverId);
                publisher.PublishAllDomainEvents(submission);
            }

            return Unit.Task;
        }
    }
}
