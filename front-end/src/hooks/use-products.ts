import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { productsService, Product } from "@/services/products-service";

// Chave única para o cache
const PRODUCTS_KEY = ["products"];

export function useProducts() {
  const queryClient = useQueryClient();

  // 1. Listar Produtos (GET)
  const { 
    data: products, 
    isLoading, 
    isError 
  } = useQuery({
    queryKey: PRODUCTS_KEY,
    queryFn: productsService.getAll,
  });

  // 2. Criar Produto (POST)
  const createMutation = useMutation({
    mutationFn: productsService.create,
    onSuccess: () => {
      // Quando criar com sucesso, invalida o cache para recarregar a lista automaticamente
      queryClient.invalidateQueries({ queryKey: PRODUCTS_KEY });
      alert("Produto criado com sucesso!");
    },
    onError: (error) => {
      console.error("Erro ao criar:", error);
      alert("Falha ao criar produto.");
    }
  });

  return {
    products,
    isLoading,
    isError,
    createProduct: createMutation.mutateAsync,
    isCreating: createMutation.isPending,
  };
}