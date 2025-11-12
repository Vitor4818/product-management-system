using MediatR;

namespace Hypesoft.Application.Commands
{
    /// <summary>
    /// Esta é a "Comanda" (o Pedido de Command) para deletar um produto.
    /// </summary>
    public class DeleteProductCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public DeleteProductCommand(string id)
        {
            Id = id;
        }
    }
}