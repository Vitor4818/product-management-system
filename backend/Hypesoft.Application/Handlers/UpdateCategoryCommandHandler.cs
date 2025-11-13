using AutoMapper;
using Hypesoft.Application.Commands;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler para processar o <see cref="UpdateCategoryCommand"/>.
    /// </summary>
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto?>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<CategoryDto?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryToUpdate = await _categoryRepository.GetByIdAsync(request.Id);

            if (categoryToUpdate == null)
            {
                return null; 
            }

            // Lógica de atualização parcial 
            if (request.Name != null)
            {
                categoryToUpdate.Name = request.Name;
            }

            // Salva as mudanças no banco.
            await _categoryRepository.UpdateAsync(categoryToUpdate);

            // Mapeia a entidade (agora atualizada) de volta para um DTO
            return _mapper.Map<CategoryDto>(categoryToUpdate);
        }
    }
}