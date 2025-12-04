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
    /// Handler para processar a query Responsável por buscar uma categoria específica pelo seu ID.
    /// Implementa Cache-Aside para otimização de leitura.
    /// </summary>
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache; 
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper, IDistributedCache cache)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Category_{request.Id}";

            // TENTA OBTER DO CACHE
            var cachedResponse = await _cache.GetAsync(cacheKey, cancellationToken);

            if (cachedResponse != null)
            {
                var cachedString = Encoding.UTF8.GetString(cachedResponse);
                return JsonSerializer.Deserialize<CategoryDto>(cachedString);
            }

            // BUSCA NO BANCO
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category == null)
            {
                return null;
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);

            // SALVA NO CACHE
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(CacheDuration);

            var jsonToCache = JsonSerializer.Serialize(categoryDto);
            var bytesToCache = Encoding.UTF8.GetBytes(jsonToCache);
            await _cache.SetAsync(cacheKey, bytesToCache, cacheOptions, cancellationToken);

            return categoryDto;
        }
    }
}