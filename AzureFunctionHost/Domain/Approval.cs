using System;

namespace AzureFunctionHost.Domain
{
    public class Approval : IIdentifiable<Guid>
    {
        protected Approval(Guid approvalId, Guid submissionId, string approverUserId, DateTimeOffset created)
        {
            ApprovalId = approvalId;
            SubmissionId = submissionId;
            UserId = approverUserId;
            Created = created;
        }

        public Guid ApprovalId { get; }

        public Guid SubmissionId { get; }

        public string UserId { get; }

        public DateTimeOffset Created { get; private set; }

        Guid IIdentifiable<Guid>.Id => ApprovalId;

        public static Approval By(string userId, Guid submissionId)
        {
            //var approvalId = new IdGenerator().NextId();
            return new Approval(submissionId, submissionId, userId, DateTimeOffset.Now);
        }
    }
}
