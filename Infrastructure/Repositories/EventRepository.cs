using Common.Entity;
using Common.Events;
using Common.Repository;
using Infrastructure.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class EventRepository:IEventRepository
{
        private  IMongoCollection<EventModel> _eventCollection;
        private readonly IMongoDatabase _mongoClient;
        private readonly string _collectionName;
        public EventRepository(IOptions<PurchaseOrderConfig> options)
        {
            MongoClient client = new MongoClient(options.Value.ConnectionString);
            _mongoClient= client.GetDatabase(options.Value.DatabaseName);
            _collectionName = options.Value.CollectionName;
            _eventCollection = _mongoClient.GetCollection<EventModel>(_collectionName);
        }
        public async Task<List<EventModel>> GetAggregate(string aggregateId)
        {
                _eventCollection = _mongoClient.GetCollection<EventModel>(_collectionName);

                return await _eventCollection.Find(e => e.AggregateIdentifier == aggregateId).ToListAsync();
        }
        private async Task SaveEventAsync(EventModel @event)
        {
            _eventCollection = _mongoClient.GetCollection<EventModel>(_collectionName);

            await _eventCollection.InsertOneAsync(@event).ConfigureAwait(true);
        }
    public async Task<List<EventModel>> GetUnprocessedEventsAsync()
    {
        _eventCollection = _mongoClient.GetCollection<EventModel>(_collectionName);

        var filter = Builders<EventModel>.Filter.Eq(e => e.IsProcessed, false);
        return await _eventCollection.Find(filter).ToListAsync();
    }

    public async Task MarkAsProcessed(string eventId)
    {
        _eventCollection = _mongoClient.GetCollection<EventModel>(_collectionName);

        var filter = Builders<EventModel>.Filter.Eq(e => e.Id, eventId);
        var update = Builders<EventModel>.Update.Set(e => e.IsProcessed, true);

        await _eventCollection.UpdateOneAsync(filter, update);
    }

    public async Task SaveEventAsync(string aggregateId, IEnumerable<DomainEventBase> baseEvents, int expectedVersion, bool byName = false, string topicName = "", string collectionName = "")
    {
        var aggregate = await GetAggregate(aggregateId);
        if (byName && expectedVersion != -1 && aggregate[^1].Version != expectedVersion)
            throw new Exception();//ConcurrencyException("expected event version wrong !!");
        var version = expectedVersion;
        foreach (var @event in baseEvents)
        {
            @event.Version = version;
            var eventModel = new EventModel
            {
                TimeStamp = DateTime.Now,
                AggregateType = nameof(PoEntity),
                AggregateIdentifier = aggregateId,
                Version = version,
                EventBaseData = @event,
                EventType = @event.GetType().Name,
                IsProcessed = IsEventAllowedToSkipProcessing(@event.GetType().Name)
            };
            version++;

            await SaveEventAsync(eventModel);
        }
    }
    public bool IsEventAllowedToSkipProcessing(string eventName) =>
        eventName switch
        {
            nameof(OrderShipped) => true,
            nameof(OrderBeingShipped) => true,
            nameof(OrderClosed) => true,
            _ => false
        };

}
