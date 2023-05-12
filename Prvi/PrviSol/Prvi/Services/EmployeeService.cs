using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Prvi.Models.Aggregations;
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
        public async Task<List<AggregationEmployee>> GetAllEmployeesByPlataAggr()
        {
            return await this.employeeCollection.Aggregate().Group(
                    PP => PP.Payement,
                    y => new AggregationEmployee{ 
                        intAgg = y.Key,
                        employeeAgg = y.OrderByDescending(x => x.Payement).First()
                    }
                    //new BsonDocument {
                    //    { "_id", "{payement: \"$payement\"}"}
                    ).ToListAsync();


        }

        public async Task<List<AggregationEmployee>> GetAllEmployeesByPlataAggrMatch(string fName)
        {
            var filter = Builders<Employee>.Filter.Eq(A => A.FirstName, fName);
            return await this.employeeCollection.Aggregate().Match(filter).Group(
                    PP => PP.Payement,
                    y => new AggregationEmployee
                    {
                        intAgg = y.Key,
                        employeeAgg = y.OrderByDescending(x => x.Payement).First()
                    }
                    //new BsonDocument {
                    //    { "_id", "{payement: \"$payement\"}"}
                    ).ToListAsync();


        }

        private async Task CreateIndex() {
            //this.employeeCollection.Indexes<Employee>();
            var indexKeysDefinition = Builders<Employee>.IndexKeys.Ascending(e => e.Payement);
            await this.employeeCollection.Indexes.CreateOneAsync(new CreateIndexModel<Employee>(indexKeysDefinition));
        }

        private async Task CreateIndexByOccupation()
        {
            //this.employeeCollection.Indexes<Employee>();
            var indexKeysDefinition = Builders<Employee>.IndexKeys.Ascending(e => e.Occupation);
            await this.employeeCollection.Indexes.CreateOneAsync(new CreateIndexModel<Employee>(indexKeysDefinition));
        }


        public async Task<Employee> GetIndexes(string jmbg) =>
            await this.employeeCollection.Find($"{{\"EmployeesOnProject.JMBG \":\'{jmbg}\'}}").FirstOrDefaultAsync();

        //private async Task createIndex(int payement) {
        //    var indexOptions = new CreateIndexOptions();
        //    var indexKeys = Builders<Employee>.IndexKeys.Ascending(x => x.Payement == payement);
        //    var myIndex = new CreateIndexModel<Employee>(indexKeys, indexOptions);
        //    await this.employeeCollection.Indexes.CreateOneAsync(myIndex);
        //}

        //private async void createIndex2(int payement)
        //{
        //    var options = new CreateIndexOptions { Unique = true };
        //    await this.employeeCollection.Indexes.CreateOneAsync("{payement: payement}", options);
        //}

        public async Task<List<Employee>> GetAllEmpByPayment(int payement) {
            CreateIndex();

            using (var cursor = this.employeeCollection.Indexes.List())
            {
                foreach (var document in cursor.ToEnumerable())
                {
                    Console.WriteLine(" ##### " + document.ToString());
                }
            }

            return await this.employeeCollection.Find(x => x.Payement == payement).ToListAsync();
        }//$"{{payement:\'{payement}\'}}"
        //ind($"{{\"employees.JMBG\":\'{jmbg}\'}}")

        public async Task<List<Employee>> GetAllEmpByOccup(string occupation)
        {
            CreateIndexByOccupation();

            using (var cursor = this.employeeCollection.Indexes.List())
            {
                foreach (var document in cursor.ToEnumerable())
                {
                    Console.WriteLine(" ##### " + document.ToString());
                }
            }

            return await this.employeeCollection.Find(x => x.Occupation == occupation).ToListAsync();
        }
        public async Task<List<Employee>> GetAllEmployeesByName(string emplName) =>
            await this.employeeCollection.Find(e => e.FirstName == emplName).ToListAsync();
        }
}
