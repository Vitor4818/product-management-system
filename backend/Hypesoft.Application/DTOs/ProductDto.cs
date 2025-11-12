namespace Hypesoft.Application.DTOs
{
    /// <summary>
    /// DTO para a entidade Product.
    /// Este é o objeto que será retornado pela API para o cliente (frontend).
    /// </summary>
    public class ProductDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryId { get; set; } = string.Empty;

        // adicionar o nome da categoria aqui Depois

    }
}