using MediatR;

namespace Hypesoft.Application.Commands
{
    /// <summary>
    /// Comando para DELETAR uma Categoria existente.
    /// Retorna 'true' se foi deletado, 'false' se não foi encontrado.
    /// </summary>
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public DeleteCategoryCommand(string id)
        {
            Id = id;
        }
    }
}