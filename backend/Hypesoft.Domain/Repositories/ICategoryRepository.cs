using Hypesoft.Domain.Entities;

namespace Hypesoft.Domain.Repositories
{
    /// <summary>
    /// Contrato para o repositório de categorias.
    /// </summary>
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(string id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(string id);
    }
}