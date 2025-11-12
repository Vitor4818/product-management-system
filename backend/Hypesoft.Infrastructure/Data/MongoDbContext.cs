using Hypesoft.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Hypesoft.Infrastructure.Data
{
    /// <summary>
    /// Gerencia a conexão com o MongoDB e expõe as coleções.
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb");
            var mongoUrl = new MongoUrl(connectionString);
            var client = new MongoClient(mongoUrl);
            _database = client.GetDatabase(mongoUrl.DatabaseName);
        }
        /// <summary>
        /// Obtém uma referência para uma coleção específica no MongoDB.
        /// O repositório genérico usa isso.
        /// </summary>
        /// <typeparam name="T">O tipo da entidade (que deve herdar de BaseEntity)</typeparam>
        /// <param name="name">O nome da coleção no Mongo (ex: "Products")</param>
        public IMongoCollection<T> GetCollection<T>(string name) where T : BaseEntity
        {
            return _database.GetCollection<T>(name);
        }
    }
}