using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Mongo;

public  class EventModel : Domain.Mongo.EventModel
{
    [BsonId]
    public Guid Id { get; set; }
}