import { apiClient } from "./api-client";

// 1. Atualizamos a interface para espelhar o C# (.NET)
export interface Product {
  id?: string; // Opcional porque na criação não tem ID ainda
  Id?: string; // .NET costuma retornar 'Id' maiúsculo
  Name: string;
  Description?: string;
  Price: number;
  StockQuantity: number;
  CategoryId: string; // Agora usamos o ID da categoria
  CategoryName?: string; // Opcional, caso o back retorne o nome para exibição
}

export const productsService = {
  getAll: async () => {
    try {
      const response = await apiClient.get<any>("/products");
      console.log("📦 DADO BRUTO:", response);

      // Lógica de blindagem para encontrar a lista
      let lista = [];
      if (Array.isArray(response)) lista = response;
      else if (response?.data) lista = response.data;
      else if (response?.items) lista = response.items;
      else if (response?.result) lista = response.result;
      else if (response?.value) lista = response.value;

      return lista;
    } catch (error) {
      console.error("Erro no GET:", error);
      return [];
    }
  },

  getById: async (id: string) => {
    return await apiClient.get<Product>(`/products/${id}`);
  },

  // 2. O create agora recebe e envia os campos exatos que o Back espera
  create: async (data: Omit<Product, "id" | "Id">) => {
    return await apiClient.post<Product>("/products", data);
  },
};