using Hypesoft.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Query para buscar TODAS as categorias.
    /// Retorna uma lista de CategoryDto.
    /// </summary>
    public record GetAllCategoriesQuery(int PageNumber = 1, int PageSize = 25)
        : IRequest<PaginatedListDto<CategoryDto>>;
}