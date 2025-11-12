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
    /// Handler para processar a query GetAllProductsQuery.
    /// Responsável por buscar todos os produtos cadastrados.
    /// </summary>
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        // O Handler precisa do repositório para buscar os dados e do mapper para traduzir os dados para o DTO.
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa uma nova instância de GetAllProductsQueryHandler.
        /// </summary>
        ///repositório de produtos para acesso aos dados.
        ///AutoMapper para traduzir as entidades para DTOs.
        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            //Busca a lista completa de entidades de produto do repositório.
            var products = await _productRepository.GetAllAsync();

            //Mapeia a lista de entidades de domínio para DTO, evitando expor as entidades
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}