using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Hypesoft.Domain.Models; 

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler para a query de métricas do dashboard.
    /// </summary>
    public class GetDashboardMetricsQueryHandler : IRequestHandler<GetDashboardMetricsQuery, DashboardMetricsDto?>
    {
        private readonly IProductRepository _productRepository;

        public GetDashboardMetricsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<DashboardMetricsDto?> Handle(GetDashboardMetricsQuery request, CancellationToken cancellationToken)
        {
            var metrics = await _productRepository.GetDashboardMetricsAsync();
            return metrics;
        }
    }
}