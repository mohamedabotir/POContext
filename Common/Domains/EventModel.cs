using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.Events;
    public  class EventModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public  string AggregateIdentifier { get; set; }
        public string AggregateType { get; set; }
        public string EventType { get; set; }
        public DomainEventBase EventBaseData { get; set; }
        public bool IsProcessed { set; get; }
        public int Version { get; set; }
}
 