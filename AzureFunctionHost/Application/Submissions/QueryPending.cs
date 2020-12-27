using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AzureFunctionHost.Domain;
using AzureFunctionHost.Infrastructure;
using MediatR;

namespace AzureFunctionHost.Application.Approvals
{
    public class QueryPending : IRequest<IEnumerable<Guid>>
    {
    }

    public class QueryPendingHandler : IRequestHandler<QueryPending, IEnumerable<Guid>>
    {
        private readonly SubmissionRepository submissions;

        public QueryPendingHandler(SubmissionRepository submissions)
        {
            this.submissions = submissions;
        }

        public Task<IEnumerable<Guid>> Handle(QueryPending request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Enumerable.Empty<Guid>());
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
    }
}
