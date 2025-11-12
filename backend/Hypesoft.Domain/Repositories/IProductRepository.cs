using Hypesoft.Domain.Entities;

namespace Hypesoft.Domain.Repositories
{
    /// <summary>
    /// Contrato específico para o repositório de produtos.
    /// Herda todos os métodos CRUD básicos de IRepository<Product>
    /// e adiciona métodos específicos de negócio para Produtos.
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryIdAsync(string categoryId);
        Task<IEnumerable<Product>> GetLowStockAsync(int threshold = 10);
    }

    
}