using MediatR;
using Hypesoft.Application.DTOs;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Query para buscar produtos por ID de Categoria, suportando paginação.
    /// Utiliza 'record' para imutabilidade.
    /// </summary>
    public record GetProductsByCategoryIdQuery : IRequest<PaginatedListDto<ProductDto>>
    {
        public string CategoryId { get; init; } = string.Empty;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 25;

        public GetProductsByCategoryIdQuery() { }

        public GetProductsByCategoryIdQuery(string categoryId, int pageNumber = 1, int pageSize = 25)
        {
            CategoryId = categoryId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}