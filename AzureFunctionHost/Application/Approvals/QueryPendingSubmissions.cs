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
    public class QueryPendingSubmissions : IRequest<IEnumerable<Guid>>
    {
    }

    public class QueryPendingSubmissionsHandler : IRequestHandler<QueryPendingSubmissions, IEnumerable<Guid>>
    {
        private readonly SubmissionRepository submissions;

        public QueryPendingSubmissionsHandler(SubmissionRepository submissions)
        {
            this.submissions = submissions;
        }

        public Task<IEnumerable<Guid>> Handle(QueryPendingSubmissions request, CancellationToken cancellationToken)
        {
                        return Task.FromResult(Enumerable.Empty<Guid>());
        }
    }

    public class PendingSubmissionDto
    {
        public PendingSubmissionDto(Submission submission)
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
