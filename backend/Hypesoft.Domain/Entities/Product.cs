namespace Hypesoft.Domain.Entities
{
    /// <summary>
    /// Entidade principal que representa um Produto.
    /// </summary>
    public class Product : BaseEntity
    {

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
        
        //Foreign key para categoria
        public string CategoryId { get; set; } = string.Empty;

        public Category? Category { get; set; }
    }
}