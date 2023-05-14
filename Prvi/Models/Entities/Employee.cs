using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prvi.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; } = String.Empty;

        [BsonElement("firstName")]
        public string FirstName { get; set; } = string.Empty;
        [BsonElement("lastName")]
        public string LastName { get; set; } = string.Empty;
        [BsonElement("jmbg")]
        public string JMBG { get; set; } = string.Empty;
        [BsonElement("occupation")]
        public string Occupation { get; set; } = string.Empty;
        [BsonElement("payement")]
        public int Payement { get; set; }
        [BsonElement("gender")]
        public string Gender { get; set; } = string.Empty;
    }
}
