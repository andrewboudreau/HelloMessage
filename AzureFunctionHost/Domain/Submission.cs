using System;

namespace AzureFunctionHost.Domain
{
    public class Submission : IIdentifiable<Guid>
    {
        protected Submission(Guid submissionId, string userId, DateTimeOffset created)
        {
            SubmissionId = submissionId;
            UserId = userId;
            Created = created;
        }

        public Guid SubmissionId { get; private set; }

        public string UserId { get; private set; }

        public DateTimeOffset Created { get; private set; }

        Guid IIdentifiable<Guid>.Id => SubmissionId;

        public Approval ApproveBy(string approver)
        {
            return Approval.By(approver, SubmissionId);
        }

        public static Submission For(string userId)
        {
            return new Submission(new IdGenerator().NextId(), userId, DateTimeOffset.Now);
        }
    }
}
