using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using Hypesoft.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hypesoft.API.Controllers
{
    /// <summary>
    /// Controlador para métricas e dados de Dashboard/Estoque.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lista produtos com estoque baixo, suportando paginação no resultado.
        /// </summary>
        /// <param name="query">Parâmetros de paginação (PageNumber e PageSize).</param>
        [HttpGet("lowstock")]
        [ProducesResponseType(typeof(PaginatedListDto<ProductDto>), StatusCodes.Status200OK)]
public async Task<IActionResult> GetLowStockProducts([FromQuery] GetLowStockProductsQuery query)
{
    // Garante que o objeto não venha nulo
    query ??= new GetLowStockProductsQuery();

    // Como as propriedades são 'init' ou o objeto é um record imutável, 
    // nós criamos uma nova instância ajustando os valores se eles vierem zerados ou negativos.
    var validatedQuery = new GetLowStockProductsQuery
    {
        PageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber,
        PageSize = query.PageSize <= 0 ? 10 : query.PageSize
    };

    // Envia a query validada para o MediatR
    var products = await _mediator.Send(validatedQuery);
    return Ok(products);
}


        /// <summary>
        /// Busca as métricas principais do dashboard (Total de Produtos e Valor Total do Estoque).
        /// </summary>
        [HttpGet("metrics")]
        [ProducesResponseType(typeof(DashboardMetricsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDashboardMetrics()
        {
            var query = new GetDashboardMetricsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Busca os dados para o gráfico de produtos por categoria.
        /// </summary>
        [HttpGet("chart")]
        [ProducesResponseType(typeof(IEnumerable<CategoryProductCountDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsPerCategoryChart()
        {
            var query = new GetProductsPerCategoryChartQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}