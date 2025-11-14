namespace Hypesoft.Domain.Models
{
    /// <summary>
    /// DTO para retornar as métricas agregadas do Dashboard.
    /// </summary>
    public class DashboardMetricsDto
    {
        public long TotalProducts { get; set; }
        public decimal TotalStockValue { get; set; }
    }
}