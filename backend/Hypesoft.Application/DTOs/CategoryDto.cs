namespace Hypesoft.Application.DTOs
{
    /// <summary>
    /// DTO para a entidade Category.
    /// Este é o objeto "limpo" que será retornado pela API.
    /// </summary>
    public class CategoryDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}