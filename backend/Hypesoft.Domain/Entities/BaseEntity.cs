using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hypesoft.Domain.Entities
{
    /// <summary>
    /// Classe base para todas as entidades do domínio.
    /// Garante que todas as entidades tenham um Id.
    /// </summary>
    public abstract class BaseEntity
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; } = string.Empty;
    }
}