using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Query para buscar UMA categoria pelo ID.
    /// Retorna um único CategoryDto.
    /// </summary>
    public class GetCategoryByIdQuery : IRequest<CategoryDto?>
    {
        public string Id { get; set; }

        public GetCategoryByIdQuery(string id)
        {
            Id = id;
        }
    }
}