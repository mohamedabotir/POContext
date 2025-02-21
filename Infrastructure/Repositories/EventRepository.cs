using Common.Repository;
using Infrastructure.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EventModel = Common.Events.EventModel;

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
        public async Task<List<EventModel>> GetAggregate(Guid aggregateId)
        {
                _eventCollection = _mongoClient.GetCollection<EventModel>(_collectionName);

                return await _eventCollection.Find(e => e.AggregateIdentifier == aggregateId).ToListAsync();
        }
        public async Task SaveEventAsync(EventModel @event)
        {
            _eventCollection = _mongoClient.GetCollection<EventModel>(_collectionName);

            await _eventCollection.InsertOneAsync(@event).ConfigureAwait(true);
        }
    }
