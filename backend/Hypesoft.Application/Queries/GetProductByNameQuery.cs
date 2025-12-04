using Hypesoft.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Esta é a Query para buscar produtos por nome.
    /// </summary>
    public record GetProductsByNameQuery(string Name, int PageNumber = 1, int PageSize = 25)
        : IRequest<PaginatedListDto<ProductDto>>;
}