using Hypesoft.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Query para buscar produtos por ID de Categoria.
    /// Retorna uma lista de ProductDto.
    /// </summary>
    public class GetProductsByCategoryIdQuery : IRequest<IEnumerable<ProductDto>>
    {
        public string CategoryId { get; set; }

        public GetProductsByCategoryIdQuery(string categoryId)
        {
            CategoryId = categoryId;
        }
    }
}