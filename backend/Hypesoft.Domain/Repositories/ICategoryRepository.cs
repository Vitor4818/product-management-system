using Hypesoft.Domain.Entities;

namespace Hypesoft.Domain.Repositories
{
    /// <summary>
    /// Contrato para o repositório de Categorias.
    /// Herda todos os métodos CRUD básicos
    /// do IRepository<Category> genérico.
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {

    }
}