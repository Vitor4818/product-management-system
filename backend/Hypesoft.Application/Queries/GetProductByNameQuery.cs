using Hypesoft.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Esta é a Query para buscar produtos por nome.
    /// </summary>
    public class GetProductsByNameQuery : IRequest<IEnumerable<ProductDto>>
    {
        /// <summary>
        /// O termo de busca para o nome.
        /// </summary>
        public string Name { get; set; }

        public GetProductsByNameQuery(string name)
        {
            Name = name;
        }
    }
}