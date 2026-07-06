using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text; 
using System; 

namespace Hypesoft.Application.Handlers
{
    public class GetLowStockProductsQueryHandler : IRequestHandler<GetLowStockProductsQuery, PaginatedListDto<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private const int LowStockThreshold = 10;

        public GetLowStockProductsQueryHandler(IProductRepository productRepository, IMapper mapper, IDistributedCache cache)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<PaginatedListDto<ProductDto>> Handle(
            GetLowStockProductsQuery request, CancellationToken cancellationToken)
        {
            // 1. TRATAMENTO DOS PARÂMETROS (Evita paginação negativa ou divisão por zero)
       var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            // GERA CHAVE UTILIZANDO AS VARIÁVEIS TRATADAS
            var cacheKey = $"LowStock_{pageNumber}_{pageSize}";

            // CONSULTA AO BANCO (Se o cache falhar)
            var allLowStockProducts = await _productRepository.GetLowStockAsync(LowStockThreshold);

            var totalCount = allLowStockProducts.Count(); 
            
            // 2. APLICA A PAGINAÇÃO COM AS VARIÁVEIS TRATADAS
            var paginatedProducts = allLowStockProducts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Mapeia para DTOs
            var productDtos = _mapper.Map<List<ProductDto>>(paginatedProducts);

            // 3. INSTANCIA O DTO COM VALORES SEGUROS
            var paginatedList = new PaginatedListDto<ProductDto>(
                productDtos,
                totalCount,
                pageNumber,
                pageSize);

            // SALVA NO CACHE
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

            var jsonToCache = JsonSerializer.Serialize(paginatedList);
            var bytesToCache = Encoding.UTF8.GetBytes(jsonToCache);
            await _cache.SetAsync(cacheKey, bytesToCache, cacheOptions, cancellationToken);
            
            return paginatedList;
        }
    }
}