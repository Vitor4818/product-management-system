using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler  para processar a <see cref="GetCategoryByIdQuery"/>.
    /// </summary>
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            // Busca a entidade no banco.
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category == null)
            {
                return null;
            }

            // Mapeia a entidade para DTO 
            return _mapper.Map<CategoryDto>(category);
        }
    }
}