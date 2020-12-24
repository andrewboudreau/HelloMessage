using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AzureFunctionHost.Infrastructure;
using MediatR;

namespace AzureFunctionHost.Application.Approvals
{
    public class ApproveAllSubmissions : IRequest
    {
        public ApproveAllSubmissions(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }
    }

    public class ApproveAllSubmissionsHandler : IRequestHandler<ApproveAllSubmissions>
    {
        private readonly SubmissionRepository submissions;
        private readonly ApprovalRepository approvals;

        public ApproveAllSubmissionsHandler(SubmissionRepository submissions, ApprovalRepository approvals)
        {
            this.submissions = submissions;
            this.approvals = approvals;
        }

        public Task<Unit> Handle(ApproveAllSubmissions request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}
