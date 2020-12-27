using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AzureFunctionHost.Domain.Events;
using AzureFunctionHost.Infrastructure;
using MediatR;

namespace AzureFunctionHost.Application.Approvals
{
    public class ApproveAllSubmissions : IRequest
    {
        public ApproveAllSubmissions(string userId)
        {
            ApproverId = userId;
        }

        public string ApproverId { get; private set; }
    }

    public class ApproveAllSubmissionsHandler : IRequestHandler<ApproveAllSubmissions>
    {
        private readonly SubmissionRepository submissions;
        private readonly IPublisher publisher;

        public ApproveAllSubmissionsHandler(SubmissionRepository submissions, IPublisher publisher)
        {
            this.submissions = submissions;
            this.publisher = publisher;
        }

        public Task<Unit> Handle(ApproveAllSubmissions request, CancellationToken cancellationToken)
        {
            var pending = submissions.Query().Where(x => x.Pending);

            foreach (var submission in pending.ToList())
            {
                submission.ApproveBy(request.ApproverId);
                publisher.PublishAllDomainEvents(submission);
            }

            return Unit.Task;
        }
    }
}
