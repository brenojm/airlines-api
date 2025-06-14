using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AirlinesAPI.Entities
{
    public class Airline
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;
        public string IATA { get; set; } = null!;
        public string ICAO { get; set; } = null!;

        [BsonIgnore]
        public List<Aircraft>? Fleet { get; set; }
    }
}
