using AutoMapper;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler (lógica de negócio) para processar a query Responsável por buscar um produto específico pelo seu ID.
    /// </summary>
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

    
        /// Inicializa uma nova instância de GetProductByIdQueryHandler
        public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// Processa a query GetProductByIdQuery.
        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            //Busca a entidade de produto do repositório.
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                //Retornar nulo sinaliza ao Controller que o recurso
                //não foi encontrado (HTTP 404).
                return null;
            }

            //Mapeia a entidade de domínio para o DTO
            //que será retornado ao cliente, em vez de expor a entidade.
            return _mapper.Map<ProductDto>(product);
        }
    }
}