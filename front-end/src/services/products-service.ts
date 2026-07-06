import { apiClient } from "./api-client";

export interface Product {
  id?: string;
  Id?: string;
  Name: string;
  Description?: string;
  Price: number;
  StockQuantity: number;
  CategoryId: string;
  CategoryName?: string;
}

export const productsService = {
  getAll: async (): Promise<Product[]> => {
    try {
      const response = await apiClient.get<any>("/products");

      console.log("📦 Resposta Products:", response);

      if (Array.isArray(response)) return response;
      if (Array.isArray(response?.data)) return response.data;
      if (Array.isArray(response?.items)) return response.items;
      if (Array.isArray(response?.result)) return response.result;
      if (Array.isArray(response?.value)) return response.value;

      console.warn("Formato inesperado da resposta:", response);
      return [];
    } catch (error) {
      console.error("Erro ao buscar produtos:", error);
      return [];
    }
  },

  getById: async (id: string): Promise<Product> => {
    return await apiClient.get<Product>(`/products/${id}`);
  },

  create: async (data: Omit<Product, "id" | "Id">): Promise<Product> => {
    return await apiClient.post<Product>("/products", data);
  },

  // 🟢 CORRIGIDO: Removido o "api/" duplicado
  update: async (id: string, data: Omit<Product, "id" | "Id">): Promise<Product> => {
    return await apiClient.put<Product>(`/products/${id}`, data);
  },

  // 🟢 CORRIGIDO: Ajustado com a barra limpa inicial
  delete: async (id: string): Promise<void> => {
    await apiClient.delete(`/products/${id}`);
  },

  getByCategory: async (categoryId: string): Promise<Product[]> => {
    try {
      const response = await apiClient.get<any>(`/products/by-category/${categoryId}`);
      
      console.log(`📦 Resposta Products Categoria [${categoryId}]:`, response);

      if (Array.isArray(response)) return response;
      if (Array.isArray(response?.data)) return response.data;
      if (Array.isArray(response?.items)) return response.items;
      if (Array.isArray(response?.result)) return response.result;
      if (Array.isArray(response?.value)) return response.value;

      return [];
    } catch (error) {
      console.error(`Erro ao buscar produtos da categoria ${categoryId}:`, error);
      return [];
    }
  }
};