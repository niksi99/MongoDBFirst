using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Prvi.Models.Db;
using Prvi.Models.Entities;

namespace Prvi.Services
{
    public class ProjectService
    {
        private readonly IMongoCollection<Project> projectCollection;

        public ProjectService(IOptions<MyMongoDB> mongoDBSettings) {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDB = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);

            projectCollection = mongoDB.GetCollection<Project>("projects");
        }

        public async Task CreateNewProject(Project p) =>
            await this.projectCollection.InsertOneAsync(p);

        public async Task<BsonDocument> GetAllProject()
        {
            var result = await this.projectCollection
               .Aggregate()
               .Unwind<Project, Employee>(i => i.EmployeesOnProject)
               .Group(
                   k => k.Id,
                   g => new
                   {
                       EmpoyeeId = g.Key,
                       FirstName = g.First().FirstName,
                       LastName = g.First().LastName,
                       Occupation = g.First().Occupation,
                       Payement = g.First().Payement
                      
                   })
               .ToListAsync();

            return result.ToBsonDocument();
        }

        

        public async Task<Project> GetProjectByProjName(string projName) =>
            await projectCollection.Find(p => p.ProjectName == projName).FirstOrDefaultAsync();

        public async Task UpdateAsync(string id, Project updatedProject) =>
            await this.projectCollection.ReplaceOneAsync(x => x.Id == id, updatedProject);

    }
}
