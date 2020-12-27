using MediatR;

namespace AzureFunctionHost.Domain.Events
{
    public class SubmissionRejected : INotification
    {
        public SubmissionRejected(Submission submission, SubmissionResponse response)
        {
            Submission = submission;
            Response = response;
        }

        public Submission Submission { get; }

        public SubmissionResponse Response { get; }
    }
}
