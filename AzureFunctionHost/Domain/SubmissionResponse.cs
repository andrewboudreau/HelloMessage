using System;

namespace AzureFunctionHost.Domain
{
    public class SubmissionResponse
    {
        private readonly bool approved;

        protected SubmissionResponse(Guid approvalId, string approverUserId, bool approved, DateTimeOffset created)
        {
            ApprovalId = approvalId;
            UserId = approverUserId;
            Created = created;
            this.approved = approved;
        }

        public Guid ApprovalId { get; }

        public string UserId { get; }

        public DateTimeOffset Created { get; private set; }

        public bool Approved => approved;

        public bool Rejected => !approved;

        public static SubmissionResponse ApprovalBy(string userId, Guid submissionId)
        {
            var approvalId = submissionId; // new IdGenerator().NextId();
            return new SubmissionResponse(approvalId, userId, true, DateTimeOffset.Now);
        }

        public static SubmissionResponse RejectionBy(string userId, Guid submissionId)
        {
            var approvalId = submissionId; // new IdGenerator().NextId();
            return new SubmissionResponse(approvalId, userId, false, DateTimeOffset.Now);
        }
    }
}
