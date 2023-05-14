using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prvi.Models.Entities;

namespace Prvi.Models.Aggregations
{
    public class UnwindProjectToEmployees
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = string.Empty;
        public string projectName {get; set; } = string.Empty;
        public Employee employeesOnProject { get; set; }
    }
}
