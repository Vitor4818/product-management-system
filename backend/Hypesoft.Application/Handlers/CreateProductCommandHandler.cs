using AutoMapper; // <-- ADICIONADO
using Hypesoft.Application.Commands;
using Hypesoft.Domain.Entities;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler para processar o CreateProductCommand.
    /// Responsável por validar, mapear e persistir um novo produto.
    /// </summary>
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, string>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper; 

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper) 
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            
            //Mapeia o Commando para a Entidade 'produto'
            var product = _mapper.Map<Product>(request);

            //Persiste a nova entidade no banco de dados.
            await _productRepository.AddAsync(product);

            //Retorna o ID do novo produto
            return product.Id;
        }
    }
}