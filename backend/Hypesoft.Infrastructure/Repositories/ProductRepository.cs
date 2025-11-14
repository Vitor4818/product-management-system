using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Hypesoft.Domain.Models; 
using System.Linq; 
using MongoDB.Driver.Linq;

namespace Hypesoft.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação concreta do repositório de produtos.
    /// Herda de Repository<Product> para ganhar todo o CRUD básico.
    /// </summary>
    public class ProductRepository : Repository<Product>, IProductRepository 
    {
        private readonly MongoDbContext _context;

        public ProductRepository(MongoDbContext context) : base(context, "Products") 
        {
            _context = context; 
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(string categoryId)
        {
            return await _collection.Find(p => p.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetLowStockAsync(int threshold = 10)
        {
            return await _collection.Find(p => p.StockQuantity < threshold).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            var filter = Builders<Product>.Filter.Regex(
                p => p.Name, 
                new BsonRegularExpression(name, "i")
            );
            return await _collection.Find(filter).ToListAsync();
        }


        // Implementa a busca de métricas usando o IAggregateFluent 
        public async Task<DashboardMetricsDto> GetDashboardMetricsAsync()
        {
            // Vamos fazer decimal * int e criar um novo campo "CalculatedStockValue"
            var projection = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                        { "CalculatedStockValue", new BsonDocument
                            {
                                { "$multiply", new BsonArray { "$Price", "$StockQuantity" } }
                            }
                        }
                    }
                }
            };
            
            // Agrupa depois de fazer a conta
            var group = new BsonDocument
            {
                { "$group", new BsonDocument
                    {
                        { "_id", BsonNull.Value },
                        { "TotalProducts", new BsonDocument { { "$sum", 1 } } },
                        { "TotalStockValue", new BsonDocument { { "$sum", "$CalculatedStockValue" } } }
                    }
                }
            };
            
            var pipeline = new[] { projection, group };

            // Executa a Agregação
            var aggregationResult = await _collection.AggregateAsync<BsonDocument>(pipeline);
            
            // Pega o primeiro e único resultado
            var result = await aggregationResult.FirstOrDefaultAsync();

            if (result == null)
            {
                return new DashboardMetricsDto { TotalProducts = 0, TotalStockValue = 0 };
            }

            // converte para DTO
            return new DashboardMetricsDto
            {
                TotalProducts = result["TotalProducts"].IsInt32
                    ? result["TotalProducts"].AsInt32
                    : result["TotalProducts"].AsInt64,

                TotalStockValue = result["TotalStockValue"].IsDecimal128
                    ? (decimal)result["TotalStockValue"].AsDecimal128
                    : (decimal)result["TotalStockValue"].ToDouble()
            };
        }


public async Task<IEnumerable<CategoryProductCountDto>> GetProductsPerCategoryAsync()
{
    var pipeline = new[]
    {
        // Agrupa produtos por categoryId
        new BsonDocument("$group", new BsonDocument
        {
            { "_id", "$CategoryId" },
            { "ProductCount", new BsonDocument("$sum", 1) }
        }),

        // Converte o _id de category para ObjectId para garantir a compatibilidade 
        new BsonDocument("$addFields", new BsonDocument
        {
            { "_id", new BsonDocument("$toObjectId", "$_id") }
        }),

        // Faz o join com a collection Categories
        new BsonDocument("$lookup", new BsonDocument
        {
            { "from", "Categories" },
            { "localField", "_id" },
            { "foreignField", "_id" },
            { "as", "categoryInfo" }
        }),

        // Extrai o nome 
        new BsonDocument("$project", new BsonDocument
        {
            { "_id", 0 },
            { "CategoryId", "$_id" },
            { "CategoryName", new BsonDocument(
                "$ifNull", new BsonArray {
                    new BsonDocument("$arrayElemAt", new BsonArray { "$categoryInfo.Name", 0 }),
                    new BsonDocument("$arrayElemAt", new BsonArray { "$categoryInfo.name", 0 })
                })
            },
            { "ProductCount", 1 }
        })
    };

    var aggregationResult = await _collection.AggregateAsync<CategoryProductCountDto>(pipeline);
    return await aggregationResult.ToListAsync();
}
    }
}