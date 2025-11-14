using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hypesoft.Application.DTOs;
using Hypesoft.Application.Queries;
using Hypesoft.Domain.Models;

namespace Hypesoft.API.Controllers
{
    /// <summary>
    /// Controlador para métricas e dados de Dashboard/Estoque.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtém a lista de produtos com a quantidade em estoque abaixo do limite (menor que 10).
        /// </summary>
        [HttpGet("low-stock")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var products = await _mediator.Send(new GetLowStockProductsQuery());
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