import { apiClient } from "./api-client";


export interface Metrics {
  totalProducts: number;
  totalStockValue: number;
}

export interface CategoryBreakdown {
  categoryId: string;
  categoryName: string;
  productCount: number;
}

export interface LowStockProduct {
  id: string;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
  categoryId: string;
}

export interface PagedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
}

// --- FUNÇÕES DE SERVIÇO ---
export const productsService = {
  
  // Nova função para buscar Métricas
  getMetrics: async (): Promise<Metrics> => {
    // Endereço hipotético na API
    return await apiClient.get<Metrics>("/dashboard/metrics"); 
  },

  // Nova função para buscar Produtos com baixo estoque
  getLowStock: async (): Promise<PagedList<LowStockProduct>> => {
    // Endereço hipotético na API, buscando a primeira página
    return await apiClient.get<PagedList<LowStockProduct>>("/products/low-stock?page=1"); 
  },

  // Nova função para buscar dados do Gráfico de Categoria
  getCategoryBreakdown: async (): Promise<CategoryBreakdown[]> => {
    return await apiClient.get<CategoryBreakdown[]>("/dashboard/chart");
  },
};