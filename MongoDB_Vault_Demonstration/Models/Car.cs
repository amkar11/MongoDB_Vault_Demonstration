using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MongoDB_Vault_Demonstration.Models
{
    public class Car
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Model { get; set; } = string.Empty;
    }
}
