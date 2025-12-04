using Hypesoft.Application.Queries;
using Hypesoft.Domain.Models;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System;
using System.Linq; 

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler para a query do gráfico de produtos por categoria, implementando Caching.
    /// </summary>
    public class GetProductsPerCategoryChartQueryHandler : IRequestHandler<GetProductsPerCategoryChartQuery, IEnumerable<CategoryProductCountDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDistributedCache _cache; // INJEÇÃO: IDistributedCache
        private const string CacheKey = "Chart_Products_Category"; // Chave fixa para o gráfico
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30); // Cache de 30 minutos

        public GetProductsPerCategoryChartQueryHandler(IProductRepository productRepository, IDistributedCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<CategoryProductCountDto>> Handle(GetProductsPerCategoryChartQuery request, CancellationToken cancellationToken)
        {
            // Tenta ler do cache
            var cachedResponse = await _cache.GetAsync(CacheKey, cancellationToken);

            if (cachedResponse != null)
            {
                var cachedString = Encoding.UTF8.GetString(cachedResponse);
                var cachedData = JsonSerializer.Deserialize<List<CategoryProductCountDto>>(cachedString);
                if (cachedData != null)
                {
                    return cachedData;
                }
            }

            // Se o cache falhar, consultar o banco de dados
            var chartData = await _productRepository.GetProductsPerCategoryAsync();

            // Salva no cache para a próxima requisição
            if (chartData.Any())
            {
                var cacheOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(CacheDuration);

                var jsonToCache = JsonSerializer.Serialize(chartData);
                var bytesToCache = Encoding.UTF8.GetBytes(jsonToCache);

                await _cache.SetAsync(CacheKey, bytesToCache, cacheOptions, cancellationToken);
            }

            // Retorna a lista de DTOs
            return chartData;
        }
    }
}