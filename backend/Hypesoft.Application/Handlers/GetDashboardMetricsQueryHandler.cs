using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Hypesoft.Domain.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json; 
using System.Text;
using System; 

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler para a query de métricas do dashboard, implementando Caching.
    /// </summary>
    public class GetDashboardMetricsQueryHandler : IRequestHandler<GetDashboardMetricsQuery, DashboardMetricsDto?>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDistributedCache _cache; 
        private const string CacheKey = "Dashboard_Metrics";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5); 

        public GetDashboardMetricsQueryHandler(IProductRepository productRepository, IDistributedCache cache) 
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<DashboardMetricsDto?> Handle(GetDashboardMetricsQuery request, CancellationToken cancellationToken)
        {
            // TENTA OBTER DO CACHE
            var cachedResponse = await _cache.GetAsync(CacheKey, cancellationToken);

            if (cachedResponse != null)
            {
                var cachedString = Encoding.UTF8.GetString(cachedResponse);
                return JsonSerializer.Deserialize<DashboardMetricsDto>(cachedString);
            }

            // CONSULTA AO BANCO (Se o cache falhar)
            var metrics = await _productRepository.GetDashboardMetricsAsync();

            if (metrics == null)
            {
                return null;
            }

            // SALVA NO CACHE
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(CacheDuration);

            var jsonToCache = JsonSerializer.Serialize(metrics);
            var bytesToCache = Encoding.UTF8.GetBytes(jsonToCache);

            await _cache.SetAsync(CacheKey, bytesToCache, cacheOptions, cancellationToken);

            // Retorna os dados
            return metrics;
        }
    }
}