using Common.Entity;
using Common.Events;
using Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Handlers
{
    public class EventSourcing : IEventSourcing<PoEntity>
    {
        public IEventRepository _eventStore { get; set; }
        public EventSourcing(IEventRepository eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task<PoEntity> GetByIdAsync(string id, string collectionName = "")
        {
            var aggregate = new PoEntity();
            var @event = (await _eventStore.GetAggregate(id)).Select(e=>e.EventBaseData);

            aggregate.ReplayEvents(@event);

            aggregate.Version = @event.Select(e => e.Version).Max();
            return aggregate;
        }

        public async Task SaveAsync(PoEntity entity, string topicName = "", string collectionName = "")
        {

            await _eventStore.SaveEventAsync(entity.PoNumber.PoNumberValue, entity.GetUncommittedEvents(), entity.Version, false, topicName, collectionName);
            entity.MarkChangesAsCommitted();
        }
    }
}
