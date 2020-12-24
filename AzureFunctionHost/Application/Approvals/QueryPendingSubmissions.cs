using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureFunctionHost.Domain;
using AzureFunctionHost.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AzureFunctionHost.Application.Approvals
{
    public class QueryPendingSubmissions : IRequest<IEnumerable<Guid>>
    {
    }

    public class QueryPendingSubmissionsHandler : IRequestHandler<QueryPendingSubmissions, IEnumerable<Guid>>
    {
        private readonly SubmissionRepository submissions;
        private readonly ApprovalRepository approvals;

        public QueryPendingSubmissionsHandler(SubmissionRepository submissions, ApprovalRepository approvals)
        {
            this.submissions = submissions;
            this.approvals = approvals;
        }

        public Task<IEnumerable<Guid>> Handle(QueryPendingSubmissions request, CancellationToken cancellationToken)
        {
            var newest = approvals.Query().OrderByDescending(x => x.Created).FirstOrDefault()?.Created ?? DateTimeOffset.Now.AddDays(-30);
            e

            return Enumerable.Empty<string>();
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
