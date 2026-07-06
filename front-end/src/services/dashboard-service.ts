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
  items: T[];       // Se a API mandar minusculo, mantenha. Se mandar maiúsculo, mude para Items
  pageNumber: number;
  totalPages: number;
  totalCount: number; // Mude para TotalCount se notar que a API manda com "T" maiúsculo
}


// --- FUNÇÕES DE SERVIÇO ---
export const productsService = {
  
  // Nova função para buscar Métricas
  getMetrics: async (): Promise<Metrics> => {
    // Endereço hipotético na API
    return await apiClient.get<Metrics>("/dashboard/metrics"); 
  },

// No seu dashboard-service.ts:
getLowStock: async (): Promise<PagedList<LowStockProduct>> => {
  // Passando as chaves que batem com as propriedades do C# (PascalCase mapeado pelo query string)
  return await apiClient.get<PagedList<LowStockProduct>>("/dashboard/lowstock?pageNumber=1&pageSize=10"); 
},

  // Nova função para buscar dados do Gráfico de Categoria
  getCategoryBreakdown: async (): Promise<CategoryBreakdown[]> => {
    return await apiClient.get<CategoryBreakdown[]>("/dashboard/chart");
  },
};