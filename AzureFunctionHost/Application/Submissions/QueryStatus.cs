using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AzureFunctionHost.Infrastructure;
using MediatR;

namespace AzureFunctionHost.Application.Submissions
{
    public class QueryStatus : IRequest<SubmissionStatus>
    {
        public QueryStatus(Guid submissionId)
        {
            SubmissionId = submissionId;
        }

        public Guid SubmissionId { get; }
    }

    public class QueryStatusHandler : IRequestHandler<QueryStatus, SubmissionStatus>
    {
        private readonly SubmissionRepository submissions;

        public QueryStatusHandler(SubmissionRepository submissions)
        {
            this.submissions = submissions;
        }

        public Task<SubmissionStatus> Handle(QueryStatus request, CancellationToken cancellationToken)
        {
            var submission = submissions.Find(request.SubmissionId);
            if (submission.Pending)
            {
                return Task.FromResult(SubmissionStatus.Pending);
            }
            else if (submission.Approved)
            {
                return Task.FromResult(SubmissionStatus.Approved);
            }
            else if (submission.Rejected)
            {
                return Task.FromResult(SubmissionStatus.Rejected);
            }

            throw new InvalidOperationException($"Unknown submission status.");
        }
    }

    public enum SubmissionStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
