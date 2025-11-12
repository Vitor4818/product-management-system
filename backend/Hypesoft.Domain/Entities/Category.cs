using Hypesoft.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hypesoft.Domain.Entities
{
    /// <summary>
    /// Representa uma Categoria de Produto no domínio.
    /// Herda de BaseEntity para obter a propriedade Id.
    /// </summary>
    public class Category : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

    }
}