import { useQuery } from "@tanstack/react-query";
import { productsService, CategoryBreakdown } from "@/services/dashboard-service";

const BREAKDOWN_KEY = ["reports", "category-breakdown"];

export function useCategoryBreakdown() {
  const { data, isLoading, isError } = useQuery<CategoryBreakdown[]>({
    queryKey: BREAKDOWN_KEY,
    queryFn: productsService.getCategoryBreakdown,
    // Relatórios gráficos geralmente mudam pouco, cache de 10 minutos
    staleTime: 1000 * 60 * 10,
    refetchOnWindowFocus: false,
  });

  return {
    data, // Retorna o array de categorias
    isLoading,
    isError,
  };
}