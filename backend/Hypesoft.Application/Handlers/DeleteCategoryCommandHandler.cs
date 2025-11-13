using Hypesoft.Domain.Repositories;
using Hypesoft.Application.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler para processar o <see cref="DeleteCategoryCommand"/>.
    /// </summary>
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            // Busca a entidade no banco.
            var categoryToDelete = await _categoryRepository.GetByIdAsync(request.Id);

            if (categoryToDelete == null)
            {
                return false; 
            }
            // Deleta do banco.
            await _categoryRepository.DeleteAsync(request.Id);
            return true;
        }
    }
}