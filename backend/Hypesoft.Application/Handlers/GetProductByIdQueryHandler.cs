using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System;

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler (lógica de negócio) para processar a query Responsável por buscar um produto específico pelo seu ID.
    /// Implementa Cache-Aside para otimização de leitura.
    /// </summary>
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache; 

        public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper, IDistributedCache cache) // CONSTRUTOR ATUALIZADO
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache; 
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Product_{request.Id}";

            // TENTA OBTER DO CACHE
            var cachedResponse = await _cache.GetAsync(cacheKey, cancellationToken);

            if (cachedResponse != null)
            {
                var cachedString = Encoding.UTF8.GetString(cachedResponse);
                return JsonSerializer.Deserialize<ProductDto>(cachedString);
            }


            // BUSCA NO BANCO
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                return null;
            }

            var productDto = _mapper.Map<ProductDto>(product);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            var jsonToCache = JsonSerializer.Serialize(productDto);
            var bytesToCache = Encoding.UTF8.GetBytes(jsonToCache);
            await _cache.SetAsync(cacheKey, bytesToCache, cacheOptions, cancellationToken);

            return productDto;
        }
    }
}