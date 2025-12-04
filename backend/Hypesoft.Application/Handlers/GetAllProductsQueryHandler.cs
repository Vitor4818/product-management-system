using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System;

namespace Hypesoft.Application.Handlers
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginatedListDto<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache; 

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper, IDistributedCache cache)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<PaginatedListDto<ProductDto>> Handle(
            GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // GERA CHAVE DE CACHE
            var cacheKey = $"Products_{request.PageNumber}_{request.PageSize}";

            var cachedResponse = await _cache.GetAsync(cacheKey, cancellationToken);

            if (cachedResponse != null)
            {
                var cachedString = Encoding.UTF8.GetString(cachedResponse);
                return JsonSerializer.Deserialize<PaginatedListDto<ProductDto>>(cachedString) ??
                       new PaginatedListDto<ProductDto>(new List<ProductDto>(), 0, request.PageNumber, request.PageSize);
            }

            // CONSULTA AO BANCO (Se o cache falhar)
            var totalCount = await _productRepository.CountAsync();

            var skip = (request.PageNumber - 1) * request.PageSize;

            var products = await _productRepository.GetPaginatedProducts(skip, request.PageSize);

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            var paginatedList = new PaginatedListDto<ProductDto>(
                productDtos,
                (int)totalCount,
                request.PageNumber,
                request.PageSize);

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