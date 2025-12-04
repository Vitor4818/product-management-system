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
    public class GetProductsByNameQueryHandler : IRequestHandler<GetProductsByNameQuery, PaginatedListDto<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache; 

        public GetProductsByNameQueryHandler(IProductRepository productRepository, IMapper mapper, IDistributedCache cache)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<PaginatedListDto<ProductDto>> Handle(
            GetProductsByNameQuery request, CancellationToken cancellationToken)
        {
            // GERA CHAVE
            var cacheKey = $"ProductsByName_{request.Name}_{request.PageNumber}_{request.PageSize}";
            var cachedResponse = await _cache.GetAsync(cacheKey, cancellationToken);

            if (cachedResponse != null)
            {
                var cachedString = Encoding.UTF8.GetString(cachedResponse);
                return JsonSerializer.Deserialize<PaginatedListDto<ProductDto>>(cachedString) ??
                       new PaginatedListDto<ProductDto>(new List<ProductDto>(), 0, request.PageNumber, request.PageSize);
            }


            // CONSULTA AO BANCO (Se o cache falhar)
            var allMatchingProducts = await _productRepository.GetByNameAsync(request.Name);
            var totalCount = allMatchingProducts.Count();
            var paginatedProducts = allMatchingProducts
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            var productDtos = _mapper.Map<List<ProductDto>>(paginatedProducts);
            var paginatedList = new PaginatedListDto<ProductDto>(
                productDtos,
                totalCount,
                request.PageNumber,
                request.PageSize);


            // SALVA NO CACHE
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

            var jsonToCache = JsonSerializer.Serialize(paginatedList);
            var bytesToCache = Encoding.UTF8.GetBytes(jsonToCache);
            await _cache.SetAsync(cacheKey, bytesToCache, cacheOptions, cancellationToken);


            // Retorna a lista paginada
            return paginatedList;
        }
    }
}