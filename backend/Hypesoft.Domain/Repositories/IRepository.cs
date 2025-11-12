using Hypesoft.Domain.Entities;
using System.Linq.Expressions;

namespace Hypesoft.Domain.Repositories
{
    /// <summary>
    /// Contrato de repositório genérico.
    /// Contém os métodos CRUD básicos que todas as entidades compartilharão.
    /// A restrição "where T : BaseEntity" garante que este repositório
    /// só pode ser usado para classes que herdam de BaseEntity.
    /// </summary>
    /// <typeparam name="T">A entidade do domínio</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}