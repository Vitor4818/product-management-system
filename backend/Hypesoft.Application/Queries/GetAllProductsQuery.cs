using Hypesoft.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Esta é a Query para buscar TODOS os produtos.
    /// </summary>
    public record GetAllProductsQuery(int PageNumber = 1, int PageSize = 25) : IRequest<PaginatedListDto<ProductDto>>;
}