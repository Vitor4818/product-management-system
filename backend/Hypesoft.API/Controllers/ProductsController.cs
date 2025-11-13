using Hypesoft.Application.Commands;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hypesoft.API.Controllers
{
    /// <summary>
    /// Endpoint da API para gerenciar Categorias.
    /// Este Controller apenas delega todo o trabalho
    /// para o MediatR seguindo o padrão CQRS.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        //Injeta o MediatR
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var productId = await _mediator.Send(command);
            // Retorna o ID do novo produto
            return Ok(new { NewProductId = productId });
        }


        /// <summary>
        /// Lista todos os produtos.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        /// <summary>
        /// Busca um produto específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do produto</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);
            if (product == null)
            {
                return NotFound(); 
            }
            return Ok(product);
        }

        /// Rota: GET /api/products/search?name=notebook
        /// </summary>
        /// <param name="name">O termo de busca para o nome do produto.</param>
        /// <returns>Uma lista de produtos que correspondem à busca.</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchProductsByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("O termo de busca (name) não pode ser vazio.");
            }
            var query = new GetProductsByNameQuery(name);
            var products = await _mediator.Send(query);
            return Ok(products);
        }

         /// <summary>
        /// Filtra produtos por um ID de categoria específico.
        /// </summary>
        /// <param name="categoryId">O ID da categoria para filtrar.</param>
        /// <returns>Uma lista de produtos que pertencem à categoria.</returns>
        [HttpGet("by-category/{categoryId}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsByCategoryId(string categoryId)
        {
            var query = new GetProductsByCategoryIdQuery(categoryId);
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="id">O ID do produto a ser atualizado (da rota)</param>
        /// <param name="command">Os novos dados do produto (do corpo)</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new { Message = "O ID da rota não corresponde ao ID do corpo da requisição." });
            }
            var success = await _mediator.Send(command);
            if (!success)
            {
                return NotFound(); 
            }
            return NoContent();
        }

        /// <summary>
        /// Deleta um produto pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do produto a ser deletado</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var command = new DeleteProductCommand(id);
            var success = await _mediator.Send(command);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
        
        
    }
}