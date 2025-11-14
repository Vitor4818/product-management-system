using Hypesoft.Application.Commands;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Hypesoft.API.Controllers
{
    /// <summary>
    /// Endpoint da API para gerenciar Categorias.
    /// Este Controller apenas delega todo o trabalho
    /// para o MediatR seguindo o padrão CQRS.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Cria uma nova categoria.
        /// </summary>
        /// <param name="command">Os dados da nova categoria.</param>
        /// <returns>A categoria recém-criada.</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            var categoryDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDto.Id }, categoryDto);
        }

        /// <summary>
        /// Busca todas as categorias.
        /// </summary>
        /// <returns>Uma lista de todas as categorias.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var categories = await _mediator.Send(query);
            return Ok(categories);
        }

        /// <summary>
        /// Busca uma categoria específica pelo ID.
        /// </summary>
        /// <param name="id">O ID da categoria.</param>
        /// <returns>A categoria encontrada.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var query = new GetCategoryByIdQuery(id);
            var category = await _mediator.Send(query);
            if (category == null)
            {
                return NotFound("Categoria não encontrada.");
            }
            return Ok(category);
        }

        /// <summary>
        /// Atualiza uma categoria existente.
        /// Suporta atualização parcial.
        /// </summary>
        /// <param name="id">O ID da categoria a ser atualizada.</param>
        /// <param name="command">Os dados a serem atualizados.</param>
        /// <returns>A categoria atualizada.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("O ID da rota e o ID do corpo da requisição não coincidem.");
            }
            var categoryDto = await _mediator.Send(command);
            if (categoryDto == null)
            {
                return NotFound("Categoria não encontrada.");
            }
            return Ok(categoryDto);
        }

        /// <summary>
        /// Deleta uma categoria pelo ID.
        /// </summary>
        /// <param name="id">O ID da categoria a ser deletada.</param>
        /// <returns>Status 204 (Sem Conteúdo) em caso de sucesso.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var command = new DeleteCategoryCommand(id);
            var success = await _mediator.Send(command);
            if (!success)
            {
                return NotFound("Categoria não encontrada.");
            }
            return NoContent();
        }
    }
}