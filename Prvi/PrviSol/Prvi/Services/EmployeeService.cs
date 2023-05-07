﻿using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Prvi.Models.Db;
using Prvi.Models.Entities;

namespace Prvi.Services
{
    public class EmployeeService
    {
        private readonly IMongoCollection<Employee> employeeCollection;

        public EmployeeService(IOptions<MyMongoDB> mongoDBSettings) {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDBs = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            this.employeeCollection = mongoDBs.GetCollection<Employee>("employees");
        }

        public async Task CreateNewEmployee(Employee e) =>
            await this.employeeCollection.InsertOneAsync(e);

        public async Task<Employee> GetEmployeeByJMBG(string jmbg) =>
            await this.employeeCollection.Find(e => e.JMBG == jmbg).FirstOrDefaultAsync();

        public async Task<Employee> GetEmployeeByFirstName(string fName) =>
            await this.employeeCollection.Find(e => e.FirstName == fName).FirstOrDefaultAsync();

        public async Task<List<Employee>> GetAllEmployees() =>
            await this.employeeCollection.Find(_ => true).ToListAsync();

        public async Task<List<Employee>> GetAllEmployeesByNameAggr(string fName) =>
            await this.employeeCollection.Aggregate().Match(x => x.FirstName == fName).ToListAsync();

        public async Task<List<Employee>> GetAllEmployeesByNameIndexes(string fName)
        {
            var filter = Builders<Employee>.Filter.Text($"{fName}");
            var trazeniZaposleni = await this.employeeCollection.Find(filter).ToListAsync();
            return trazeniZaposleni;
        }

        public async Task<List<Employee>> GetAllEmployeesByPayementIndexes(int payement)
        {
            var filter = Builders<Employee>.Filter.Eq(p => p.Payement, payement);
            var trazeniZaposleni = await this.employeeCollection.Find(filter).ToListAsync();
            return trazeniZaposleni;
        }


        //await this.employeeCollection.Find($"{{$search: \"{fName}\"}}").ToListAsync();
        //{$text: {$search: "Noah"}}, {score: {$meta: "textScore"}})
        //ind($"{{\"employees.JMBG\":\'{jmbg}\'}}")
        public List<BsonDocument> GetAllEmployeesByPlataAggr()
        {
            return this.employeeCollection.Aggregate().Group(
                    new BsonDocument {
                        { "_id", "$Payement"}
                    }).ToList();
            
        }

        public async Task CreateIndex() {
            //this.employeeCollection.Indexes<Employee>();
            var indexKeysDefinition = Builders<Employee>.IndexKeys.Ascending(e => e.Payement);
            await this.employeeCollection.Indexes.CreateOneAsync(new CreateIndexModel<Employee>(indexKeysDefinition));
        }


        public async Task<Employee> GetIndexes(string jmbg) =>
            await this.employeeCollection.Find($"{{\"EmployeesOnProject.JMBG \":\'{jmbg}\'}}").FirstOrDefaultAsync();
        

        public async Task<List<Employee>> GetAllEmployeesByName(string emplName) =>
            await this.employeeCollection.Find(e => e.FirstName == emplName).ToListAsync();
    }
}
