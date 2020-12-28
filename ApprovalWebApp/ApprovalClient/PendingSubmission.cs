using System;

namespace ApprovalWebApp
{
    public class PendingSubmission
    {
        public PendingSubmission(Guid submissionId, DateTimeOffset created, string userId)
        {
            SubmissionId = submissionId;
            Created = created;
            UserId = userId;
        }

        public Guid SubmissionId { get; set; }

        public DateTimeOffset Created { get; set; }

        public string UserId { get; set; }
    }
}
