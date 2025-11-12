using Hypesoft.Application.Commands;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Application.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            //Busca o produto do banco (exatamente como antes)
            var productToUpdate = await _productRepository.GetByIdAsync(request.Id);

            if (productToUpdate == null)
            {
                //Caso o produto não exista
                return false;
            }

            //Análisa o corpo da request e atualiza somente os campos que estão presente no body
            //evita de ter que mandar todo o json do objeto para atualizar
            if (request.Name != null)
            {
                productToUpdate.Name = request.Name;
            }

            if (request.Description != null)
            {
                productToUpdate.Description = request.Description;
            }

            if (request.Price.HasValue)
            {
                productToUpdate.Price = request.Price.Value;
            }

            if (request.StockQuantity.HasValue)
            {
                productToUpdate.StockQuantity = request.StockQuantity.Value;
            }
            
            if (request.CategoryId != null)
            {
                productToUpdate.CategoryId = request.CategoryId;
            }
            await _productRepository.UpdateAsync(productToUpdate);

            return true;
        }
    }
}