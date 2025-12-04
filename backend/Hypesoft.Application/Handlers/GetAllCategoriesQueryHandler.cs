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
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, PaginatedListDto<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache; 

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper, IDistributedCache cache)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<PaginatedListDto<CategoryDto>> Handle(
            GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            // GERA CHAVE DE CACHE
            var cacheKey = $"Categories_{request.PageNumber}_{request.PageSize}";
            var cachedResponse = await _cache.GetAsync(cacheKey, cancellationToken);

            if (cachedResponse != null)
            {
                var cachedString = Encoding.UTF8.GetString(cachedResponse);
                return JsonSerializer.Deserialize<PaginatedListDto<CategoryDto>>(cachedString) ??
                       new PaginatedListDto<CategoryDto>(new List<CategoryDto>(), 0, request.PageNumber, request.PageSize);
            }

            // CONSULTA AO BANCO (Se o cache falhar)
            var allCategories = await _categoryRepository.GetAllAsync(); 

            var totalCount = allCategories.Count();
            var paginatedCategories = allCategories
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            var categoryDtos = _mapper.Map<List<CategoryDto>>(paginatedCategories);

            var paginatedList = new PaginatedListDto<CategoryDto>(
                categoryDtos,
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