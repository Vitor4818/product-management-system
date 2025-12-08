import { useQuery } from "@tanstack/react-query";
import { productsService, PagedList, LowStockProduct } from "@/services/dashboard-service";

// Chave composta para facilitar cache: ["products", "low-stock"]
const LOW_STOCK_KEY = ["products", "low-stock"];

export function useLowStock() {
  const { data, isLoading, isError } = useQuery<PagedList<LowStockProduct>>({
    queryKey: LOW_STOCK_KEY,
    queryFn: productsService.getLowStock,
    // Produtos acabando é algo urgente, então o cache é menor (1 minuto)
    staleTime: 1000 * 60 * 1, 
  });

  return {
    data, // Retorna o objeto { items: [], totalCount: ... }
    isLoading,
    isError,
  };
}