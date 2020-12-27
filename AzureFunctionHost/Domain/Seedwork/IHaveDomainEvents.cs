using System.Collections.Generic;
using MediatR;

namespace AzureFunctionHost.Domain
{
    public interface IHaveDomainEvents
    {
        IEnumerable<INotification> DomainEvents { get; }
    }
}