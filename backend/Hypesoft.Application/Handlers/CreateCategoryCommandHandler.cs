using AutoMapper;
using Hypesoft.Application.Commands;
using Hypesoft.Application.DTOs;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler para processar o <see cref="CreateCategoryCommand"/>.
    /// </summary>
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            // Mapeia Command para Entidade
            var category = _mapper.Map<Category>(request);

            // Salva a nova entidade no banco.
            await _categoryRepository.AddAsync(category);

            // Mapeia a entidade de volta para um DTO
            return _mapper.Map<CategoryDto>(category);
        }
    }
}