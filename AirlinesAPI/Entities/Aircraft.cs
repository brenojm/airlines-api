using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AirlinesAPI.Entities
{
    public class Aircraft
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string MSN { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Registration { get; set; } = null!;

        public string? AirlineId { get; set; }
    }
}
