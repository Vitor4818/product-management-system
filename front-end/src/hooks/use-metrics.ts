import { useQuery } from "@tanstack/react-query";
import { productsService, Metrics } from "@/services/dashboard-service";

const METRICS_KEY = ["metrics"];

export function useMetrics() {
  const { data, isLoading, isError } = useQuery<Metrics>({
    queryKey: METRICS_KEY,
    queryFn: productsService.getMetrics,
    // Como são totais (KPIs), não precisamos revalidar a cada segundo.
    // 5 minutos de cache é um bom padrão.
    staleTime: 1000 * 60 * 5, 
    refetchOnWindowFocus: false,
  });

  return {
    metrics: data,
    isLoading,
    isError,
  };
}