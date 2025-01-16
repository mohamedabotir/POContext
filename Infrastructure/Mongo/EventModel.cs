using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Mongo;

public  class EventModel : Common.Events.EventModel
{
    [BsonId]
    public Guid Id { get; set; }
}