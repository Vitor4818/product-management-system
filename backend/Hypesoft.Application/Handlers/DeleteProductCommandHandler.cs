using Hypesoft.Application.Commands;
using Hypesoft.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hypesoft.Application.Handlers
{
    /// <summary>
    /// Handler para processar DeleteProductCommand.
    /// </summary>
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // Handler verifica se o produto existe antes de tentar deletar.
            var productToDelete = await _productRepository.GetByIdAsync(request.Id);

            if (productToDelete == null)
            {
                return false; 
            }

            // Faz o Delete
            await _productRepository.DeleteAsync(request.Id);

            return true;
        }
    }
}