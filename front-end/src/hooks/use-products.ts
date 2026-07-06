import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { productsService, Product } from "@/services/products-service";

const PRODUCTS_KEY = ["products"];

export function useProducts() {
  const queryClient = useQueryClient();

  // Sempre busca todos os produtos globalmente
  const {
    data: products = [],
    isLoading,
    isError,
    error,
  } = useQuery<Product[]>({
    queryKey: PRODUCTS_KEY,
    queryFn: productsService.getAll,
  });

  // Criar Produto
  const createMutation = useMutation({
    mutationFn: productsService.create,
    onSuccess: () => {
      // Invalida a lista de produtos e o breakdown de categorias para atualizar os contadores
      queryClient.invalidateQueries({ queryKey: PRODUCTS_KEY });
      queryClient.invalidateQueries({ queryKey: ["category-breakdown"] });
    },
  });

  // Atualizar Produto
  const updateMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: Omit<Product, "id" | "Id"> }) => 
      productsService.update(id, data),
    onSuccess: () => {
      // 🔥 FORÇA O RE-FETCH IMEDIATO DA LISTA (Ignora o staleTime de 1 min)
      queryClient.invalidateQueries({ queryKey: PRODUCTS_KEY });
      
      // 📊 Invalida o gráfico caso o preço ou estoque altere o breakdown
      queryClient.invalidateQueries({ queryKey: ["category-breakdown"] });
    },
  });

  // Excluir Produto
  const deleteMutation = useMutation({
    mutationFn: productsService.delete,
    onSuccess: () => {
      // 🔥 Remove da tabela no mesmo milissegundo
      queryClient.invalidateQueries({ queryKey: PRODUCTS_KEY });
      
      // 📊 SUMIR DO GRÁFICO: Avisa o hook do gráfico para atualizar imediatamente!
      // (Substitua "category-breakdown" pela chave exata que está dentro do hook do seu gráfico se for diferente)
      queryClient.invalidateQueries({ queryKey: ["category-breakdown"] });
    },
  });

  return {
    products,
    isLoading,
    isError,
    error,
    createProduct: createMutation.mutateAsync,
    isCreating: createMutation.isPending,
    updateProduct: updateMutation.mutateAsync,
    isUpdating: updateMutation.isPending,
    deleteProduct: deleteMutation.mutateAsync,
    isDeleting: deleteMutation.isPending,
  };
} 