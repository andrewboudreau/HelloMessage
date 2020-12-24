using System.Collections.Generic;
using AzureFunctionHost.Domain;

namespace AzureFunctionHost.Application.Services
{
    public interface IRepository<TId, T>
        where T : IIdentifiable<TId>
    {
        T Find(TId id);

        void Add(T entity);

        IEnumerable<T> Query();
    }
}
