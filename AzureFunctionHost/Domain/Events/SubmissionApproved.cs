using MediatR;

namespace AzureFunctionHost.Domain.Events
{
    public class SubmissionApproved : INotification
    {
        public SubmissionApproved(string userId, Submission submission)
        {
            UserId = userId;
            Submission = submission;
        }

        public string UserId { get; }

        public Submission Submission { get; }
    }
}
