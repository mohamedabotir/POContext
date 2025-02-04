using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Mongo;

public  class EventModel : Common.Events.EventModel
{
    [BsonId]
    public Guid Id { get; set; }
}