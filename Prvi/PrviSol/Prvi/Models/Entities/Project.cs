using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prvi.Models.Entities
{
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("projectName")]
        public string ProjectName { get; set; } = string.Empty;
        [BsonElement("employeesOnProject")]
        public List<Employee> EmployeesOnProject { get; set; } = new List<Employee>();
    }
}
