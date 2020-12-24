using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AzureFunctionHost.Infrastructure;
using MediatR;

namespace AzureFunctionHost.Application.Submissions
{
    public class QuerySubmissionStatus : IRequest<bool>
    {
        public QuerySubmissionStatus(Guid submissionId)
        {
            SubmissionId = submissionId;
        }

        public Guid SubmissionId { get; }
    }

    public class CheckStatusHandler : IRequestHandler<QuerySubmissionStatus, bool>
    {
        private readonly SubmissionRepository submissions;
        private readonly ApprovalRepository approvals;

        public CheckStatusHandler(SubmissionRepository submissions, ApprovalRepository approvals)
        {
            this.submissions = submissions;
            this.approvals = approvals;
        }

        public Task<bool> Handle(QuerySubmissionStatus request, CancellationToken cancellationToken)
        {
            var submission = submissions.Find(request.SubmissionId);

            if (approvals.Query().Any(x => x.Created > submission.Created))
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
