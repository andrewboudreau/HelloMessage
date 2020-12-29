using System;

namespace ApprovalWebApp
{
    public record SubmissionAudit(Guid SubmissionId, DateTimeOffset SubmissionCreated, string SubmitterId, string ApproverId, DateTimeOffset ResponseCreated, bool Approved);
}
