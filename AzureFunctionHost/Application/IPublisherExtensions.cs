using System.Threading;
using AzureFunctionHost.Domain;
using MediatR;

namespace AzureFunctionHost.Application.Approvals
{
    public static class IPublisherExtensions
    {
        public static void PublishAllDomainEvents(this IPublisher publisher, IHaveDomainEvents aggregate, CancellationToken? cancellationToken = default)
        {
            foreach (var @event in aggregate.DomainEvents)
            {
                publisher.Publish(@event, cancellationToken.GetValueOrDefault());
            }
        }
    }
}
