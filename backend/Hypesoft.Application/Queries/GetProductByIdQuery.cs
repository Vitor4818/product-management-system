using Hypesoft.Application.DTOs;
using MediatR;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Esta é a Quey para buscar UM produto.
    /// </summary>
    public class GetProductByIdQuery : IRequest<ProductDto?>
    {
        /// <summary>
        /// Para buscar por ID, o pedido PRECISA carregar o ID.
        /// </summary>
        public string Id { get; set; }

        public GetProductByIdQuery(string id)
        {
            Id = id;
        }
    }
}