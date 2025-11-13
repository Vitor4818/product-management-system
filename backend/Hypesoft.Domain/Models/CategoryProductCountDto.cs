using MongoDB.Bson;

namespace Hypesoft.Domain.Models
{
    /// <summary>
    /// DTO para o gráfico de "Produtos por Categoria".
    /// Retorna uma contagem simples de produtos para cada categoria.
    /// </summary>
    public class CategoryProductCountDto
    {
         public ObjectId CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
    }
}