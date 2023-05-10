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
        private readonly IMongoCollection<Employee> employeeCollection;
        public ProjectService(IOptions<MyMongoDB> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDB = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);

            projectCollection = mongoDB.GetCollection<Project>("projects");
            this.employeeCollection = mongoDB.GetCollection<Employee>("employees");
        }

        public async Task CreateNewProject(Project p) =>
            await this.projectCollection.InsertOneAsync(p);

        public async Task<List<Project>> GetAllProject() =>
            await this.projectCollection.Find(_ => true).ToListAsync();

        public async Task<Project> GetProjectByProjName(string projName) =>
            await projectCollection.Find(p => p.ProjectName == projName).FirstOrDefaultAsync();

        public async Task UpdateAsync(string id, Project updatedProject) =>
            await this.projectCollection.ReplaceOneAsync(x => x.Id == id, updatedProject);

        public async Task<Project> GetProjectByEmployeeJMBG(string jmbg)
        { ////ind($"{{\"employees.jmbg\":\'{jmbg}\'}}")
            var proj = await this.projectCollection.Find($"{{\"employeesOnProject.jmbg\":\'{jmbg}\'}}").FirstOrDefaultAsync();
            return proj;
        }
            
    }
}