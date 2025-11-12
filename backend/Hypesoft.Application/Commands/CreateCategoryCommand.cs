using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Commands
{
    /// <summary>
    /// Comando para criar uma nova Categoria.
    /// Retorna a categoria recém-criada como um CategoryDto.
    /// </summary>
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        public string Name { get; set; } = string.Empty;
    }
}