using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Commands
{
    /// <summary>
    /// Comando para ATUALIZAR uma Categoria existente.
    /// Retorna o DTO da categoria atualizada.
    /// </summary>
    public class UpdateCategoryCommand : IRequest<CategoryDto?>
    {
        public string Id { get; set; } = string.Empty;
        // Name é nulável para suportar atualização parcial
        public string? Name { get; set; }
    }
}