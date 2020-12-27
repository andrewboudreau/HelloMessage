using MediatR;

namespace AzureFunctionHost.Domain.Events
{
    public class SubmissionApproved : INotification
    {
        public SubmissionApproved(Submission submission, SubmissionResponse response)
        {
            Submission = submission;
            Response = response;
        }

        public Submission Submission { get; }

        public SubmissionResponse Response { get; }
    }
}
