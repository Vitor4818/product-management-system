using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hypesoft.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação concreta do repositório de produtos.
    /// Herda de Repository<Product> para ganhar todo o CRUD básico.
    /// </summary>
    public class ProductRepository : Repository<Product>, IProductRepository 
    {
        public ProductRepository(MongoDbContext context) : base(context, "Products") 
        {
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(string categoryId)
        {
            return await _collection.Find(p => p.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetLowStockAsync(int threshold = 10)
        {
            return await _collection.Find(p => p.StockQuantity < threshold).ToListAsync();
        }
    }
}