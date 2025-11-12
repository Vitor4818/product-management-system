using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Hypesoft.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação genérica do IRepository.
    /// Contém todo o CRUD básico para qualquer entidade que herde de BaseEntity.
    /// </summary>
    /// <typeparam name="T">A entidade</typeparam>
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public Repository(MongoDbContext context, string collectionName)
        {
            _collection = context.GetCollection<T>(collectionName);
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
            }
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, entity.Id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}