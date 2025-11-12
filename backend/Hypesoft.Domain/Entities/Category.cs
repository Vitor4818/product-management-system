namespace Hypesoft.Domain.Entities
{
    /// <summary>
    /// Entidade que representa uma Categoria de produto.
    /// </summary>
    public class Category : BaseEntity
    {

        public string Name { get; set; } = string.Empty;

        //Uma categoria pode ter vários produtos
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}