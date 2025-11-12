using Hypesoft.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Esta é a "Comanda" (o Pedido de Query) para buscar TODOS os produtos.
    /// </summary>
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {

    }
}