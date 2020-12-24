using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using AzureFunctionHost.Application.Services;
using AzureFunctionHost.Domain;

namespace AzureFunctionHost.Infrastructure
{
    public class ApprovalRepository : ConcurrentDictionaryRepository<Approval>
    {
    }

    public class SubmissionRepository : ConcurrentDictionaryRepository<Submission>
    {
    }

    public class ConcurrentDictionaryRepository<T> : IRepository<Guid, T>
        where T : IIdentifiable<Guid>
    {
        private readonly ConcurrentDictionary<Guid, T> Collection = new ConcurrentDictionary<Guid, T>();

        public void Add(T entity)
        {
            Collection.TryAdd(entity.Id, entity);
        }

        public T Find(Guid id)
        {
            return Collection[id];
        }

        public IEnumerable<T> Query()
        {
            return Collection.Values;
        }
    }
}
