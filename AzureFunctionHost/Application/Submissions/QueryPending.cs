using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AzureFunctionHost.Domain;
using AzureFunctionHost.Infrastructure;
using MediatR;

namespace AzureFunctionHost.Application.Approvals
{
    public class QueryPending : IRequest<PendingSubmission[]>
    {
    }

    public class QueryPendingHandler : IRequestHandler<QueryPending, PendingSubmission[]>
    {
        private readonly SubmissionRepository submissions;

        public QueryPendingHandler(SubmissionRepository submissions)
        {
            this.submissions = submissions;
        }

        public Task<PendingSubmission[]> Handle(QueryPending request, CancellationToken cancellationToken)
        {
            var results = submissions.Query()
                .Where(x => x.Pending)
                .Select(PendingSubmission.Create)
                .ToArray();

            return Task.FromResult(results);
        }
    }

    public class PendingSubmission
    {
        public PendingSubmission(Submission submission)
        {
            SubmissionId = submission.SubmissionId;
            Created = submission.Created;
            UserId = submission.UserId;
        }
        public Guid SubmissionId { get; }

        public DateTimeOffset Created { get; }

        public string UserId { get; }

        public static PendingSubmission Create(Submission submission)
        {
            return new PendingSubmission(submission);
        }
    }
}
