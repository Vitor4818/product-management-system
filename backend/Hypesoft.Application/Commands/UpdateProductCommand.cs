using MediatR;

namespace Hypesoft.Application.Commands
{
    public class UpdateProductCommand : IRequest<bool>
    {
        // O ID do produto que será atualizado NÃO pode ser nulo.
        public string Id { get; set; } = string.Empty;

        public string? Name { get; set; } 
        public string? Description { get; set; }
        public decimal? Price { get; set; } 
        public int? StockQuantity { get; set; }
        public string? CategoryId { get; set; }
            }
}