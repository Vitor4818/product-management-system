using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using Hypesoft.Infrastructure.Data;

namespace Hypesoft.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação do repositório de Categorias.
    /// Herda de Repository para ganhar todo o CRUD básico (GetById, AddAsync, etc.)
    /// e implementa o Contrato ICategoryRepository.
    /// </summary>
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        /// </summary>
        public CategoryRepository(MongoDbContext context) 
            : base(context, "Categories") 
        {

        }

    }
}