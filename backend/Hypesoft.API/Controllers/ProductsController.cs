using Hypesoft.Application.Commands;
using Hypesoft.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hypesoft.API.Controllers
{
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