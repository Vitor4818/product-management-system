using Hypesoft.Application.Commands;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
<<<<<<< Updated upstream
=======
using Microsoft.AspNetCore.Authorization;
>>>>>>> Stashed changes

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
            return Ok(new { NewProductId = productId });
        }


        /// <summary>
        /// Lista todos os produtos com suporte a paginação.
        /// </summary>
        /// <param name="query">Parâmetros de paginação (PageNumber e PageSize).</param>
        /// <returns>Uma lista paginada de produtos.</returns>
        [HttpGet]
        [Authorize(Roles = "admin")] // Apenas usuários com a role "admin" podem listar
        [ProducesResponseType(typeof(PaginatedListDto<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQuery query)
        {
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

        /// <summary>
        /// Busca produtos pelo nome, suportando paginação no resultado.
        /// </summary>
        /// <param name="query">O termo de busca e os parâmetros de paginação.</param>
        [HttpGet("search")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(PaginatedListDto<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductsByName([FromQuery] GetProductsByNameQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.Name))
            {
                return BadRequest("O termo de busca (Name) não pode ser vazio.");
            }
            var products = await _mediator.Send(query);
            return Ok(products);
        }


        /// <summary>
        /// Filtra produtos por um ID de categoria específico.
        /// </summary>
        /// <param name="categoryId">O ID da categoria para filtrar.</param>
        /// <returns>Uma lista de produtos que pertencem à categoria.</returns>
        [HttpGet("by-category/{categoryId}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(PaginatedListDto<ProductDto>), StatusCodes.Status200OK)]
        // Note que a query AGORA SÓ PRECISA SER PASSADA.
        public async Task<IActionResult> GetProductsByCategoryId([FromRoute] string categoryId, [FromQuery] GetProductsByCategoryIdQuery query)
        {
            var products = await _mediator.Send(query with { CategoryId = categoryId });
            return Ok(products);
        }

<<<<<<< Updated upstream
        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="id">O ID do produto a ser atualizado (da rota)</param>
        /// <param name="command">Os novos dados do produto (do corpo)</param>
        [HttpPut("{id}")]
=======
                /// <summary>
                /// Atualiza um produto existente.
                /// </summary>
                /// <param name="id">O ID do produto a ser atualizado (da rota)</param>
                /// <param name="command">Os novos dados do produto (do corpo)</param>
                [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        
=======
>>>>>>> Stashed changes
        
    }
}