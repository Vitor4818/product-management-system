using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler para executar a consulta de produtos com estoque baixo.
    /// Retorna uma lista de ProductDto.
    /// </summary>
    public class GetLowStockProductsQueryHandler : IRequestHandler<GetLowStockProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private const int LowStockThreshold = 10; 

        public GetLowStockProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetLowStockProductsQuery request, CancellationToken cancellationToken)
        {
            //processa a query
            var products = await _productRepository.GetLowStockAsync(LowStockThreshold);
            // usa mapper para converter em DTO
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}