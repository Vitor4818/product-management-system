using Hypesoft.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Query para buscar TODAS as categorias.
    /// Retorna uma lista de CategoryDto.
    /// </summary>
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
    {
    }
}