using Hypesoft.Domain.Models; // <-- O DTO/Modelo que o Domínio define
using MediatR;
using System.Collections.Generic;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Query para buscar os dados do gráfico de produtos por categoria.
    /// Retorna uma lista de CategoryProductCountDto.
    /// </summary>
    public class GetProductsPerCategoryChartQuery : IRequest<IEnumerable<CategoryProductCountDto>>
    {
    }
}