using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AzureFunctionHost.Application.Services;
using AzureFunctionHost.Domain;

namespace AzureFunctionHost.Infrastructure
{
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

        public bool Exists(Guid id)
        {
            return Collection.ContainsKey(id);
        }

        public bool Exists(Func<T, bool> predicate)
        {
            return Collection.Values.Any(predicate);
        }

        public IEnumerable<T> Query()
        {
            return Collection.Values;
        }
    }
}
