using MongoDB.Driver;

namespace todoList.Services
{
    
    public class MongoDbService {

       

        private readonly IMongoDatabase _database;

        public MongoDbService(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
        
    }

    
   
}
