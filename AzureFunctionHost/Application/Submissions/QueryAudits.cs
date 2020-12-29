using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AzureFunctionHost.Domain;
using AzureFunctionHost.Infrastructure;
using MediatR;

namespace AzureFunctionHost.Application.Approvals
{
    public class QueryAudits : IRequest<SubmissionAudit[]>
    {
    }

    public class QueryAuditsHandler : IRequestHandler<QueryAudits, SubmissionAudit[]>
    {
        private readonly SubmissionRepository submissions;

        public QueryAuditsHandler(SubmissionRepository submissions)
        {
            this.submissions = submissions;
        }

        public Task<SubmissionAudit[]> Handle(QueryAudits request, CancellationToken cancellationToken)
        {
            var results = submissions.Query()
                .Where(x => !x.Pending)
                .Select(SubmissionAudit.Create)
                .ToArray();

            return Task.FromResult(results);
        }
    }

    public class SubmissionAudit
    {
        protected SubmissionAudit(Guid submissionId, DateTimeOffset submissionCreated, string submitterId, string approverId, DateTimeOffset responseCreated, bool approved)
        {
            SubmissionId = submissionId;
            SubmissionCreated = submissionCreated;
            SubmitterId = submitterId;

            ApproverId = approverId;
            ResponseCreated = responseCreated;
            Approved = approved;
        }

        public Guid SubmissionId { get; }

        public DateTimeOffset SubmissionCreated { get; }

        public string SubmitterId { get; }

        public string ApproverId { get; }

        public DateTimeOffset ResponseCreated { get; }

        public bool Approved { get; }

        public static SubmissionAudit Create(Submission submission)
        {
            if (submission.Response is null || submission.Pending)
            {
                throw new ArgumentException($"Submission must not be pending must be 'false' creating a {nameof(SubmissionAudit)}.", nameof(submission));
            }

            return new SubmissionAudit(
                submission.SubmissionId,
                submission.Created,
                submission.UserId,
                submission.Response.UserId,
                submission.Response.Created,
                submission.Response.Approved);
        }
    }
}
