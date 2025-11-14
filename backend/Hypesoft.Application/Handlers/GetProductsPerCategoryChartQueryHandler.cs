using Hypesoft.Application.Queries;
using Hypesoft.Domain.Models;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler para a query do gráfico de produtos por categoria.
    /// </summary>
    public class GetProductsPerCategoryChartQueryHandler : IRequestHandler<GetProductsPerCategoryChartQuery, IEnumerable<CategoryProductCountDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsPerCategoryChartQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<CategoryProductCountDto>> Handle(GetProductsPerCategoryChartQuery request, CancellationToken cancellationToken)
        {
            var chartData = await _productRepository.GetProductsPerCategoryAsync();

            // Retorna a lista de DTOs que o Repositório preparou
            return chartData;
        }
    }
}