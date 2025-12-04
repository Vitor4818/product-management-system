using Hypesoft.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Query para buscar produtos com estoque abaixo do limite (10).
    /// </summary>
    public record GetLowStockProductsQuery(int PageNumber = 1, int PageSize = 25)
        : IRequest<PaginatedListDto<ProductDto>>;
}