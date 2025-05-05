using Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events
{
    public interface IEventSourcing<T> where T : AggregateRoot
    {
        Task SaveAsync(T aggregate, string topicName = "", string collectionName = "");
        Task<T> GetByIdAsync(string aggregateId, string collectionName = "");
    }
}
