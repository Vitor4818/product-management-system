using Hypesoft.Domain.Models; // <-- O DTO/Modelo que o Domínio define
using MediatR;

namespace Hypesoft.Application.Queries
{
    /// <summary>
    /// Query para buscar as métricas principais do Dashboard.
    /// Retorna um DashboardMetricsDto ou nulo.
    /// </summary>
    public class GetDashboardMetricsQuery : IRequest<DashboardMetricsDto?>
    {
    }
}