using System;
using System.Collections.Generic;

using AzureFunctionHost.Domain.Events;
using MediatR;

namespace AzureFunctionHost.Domain
{
    public class Submission : IIdentifiable<Guid>, IHaveDomainEvents
    {
        private readonly List<INotification> domainEvents;

        protected Submission(Guid submissionId, string userId, DateTimeOffset created, SubmissionResponse? response = null)
        {
            SubmissionId = submissionId;
            UserId = userId;
            Created = created;
            Response = response;

            domainEvents = new List<INotification>();
        }

        public Guid SubmissionId { get; private set; }

        public string UserId { get; private set; }

        public DateTimeOffset Created { get; private set; }

        public SubmissionResponse? Response { get; private set; }

        /// <summary>
        /// Gets a boolean which is true when a <see cref="Submission"/> has a response.
        /// </summary>
        public bool Pending => Response is null;

        /// <summary>
        /// Gets a boolean which is true when a <see cref="Submission"/> has a response and it's not 'Approved'.
        /// </summary>
        public bool Rejected => !Pending && !Response!.Approved;

        /// <summary>
        /// Gets a boolean which is true when a <see cref="Submission"/> has a response and is 'Approved'./
        /// </summary>
        public bool Approved => !Pending && Response!.Approved;

        Guid IIdentifiable<Guid>.Id => SubmissionId;

        IEnumerable<INotification> IHaveDomainEvents.DomainEvents => domainEvents;

        public void ApproveBy(string approver)
        {
            Response = SubmissionResponse.ApprovalBy(approver, SubmissionId);
            domainEvents.Add(new SubmissionApproved(this, Response));
        }

        public void RejectBy(string rejector)
        {
            Response = SubmissionResponse.RejectionBy(rejector, SubmissionId);
            domainEvents.Add(new SubmissionRejected(this, Response));
        }

        public static Submission For(string userId)
        {
            return new Submission(new IdGenerator().NextId(), userId, DateTimeOffset.Now);
        }
    }
}
