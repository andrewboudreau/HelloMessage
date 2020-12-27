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
        public CheckStatusHandler(SubmissionRepository submissions)
        {
            this.submissions = submissions;
        }

        public Task<bool> Handle(QuerySubmissionStatus request, CancellationToken cancellationToken)
        {
            var submission = submissions.Find(request.SubmissionId);
            return Task.FromResult(submission.Approved);
        }
    }
}
