using Hypesoft.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Query para buscar produtos com estoque abaixo do limite (10).
    /// </summary>
    public class GetLowStockProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
    }
}