using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Prvi.Models.Entities
{
    public class Project
    {
        [JsonIgnore]
        //[System.Text.Json.Serialization.JsonIgnore] //add this one
        [BsonIgnoreIfNull]
        [BsonId]
        //[Ignore]
        //[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("projectName")]
        public string ProjectName { get; set; } = string.Empty;
        [BsonElement("employeesOnProject")]
        public List<Employee> EmployeesOnProject { get; set; } = new List<Employee>();
    }
}
