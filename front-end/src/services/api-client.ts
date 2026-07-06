import { getSession } from "next-auth/react";

const BASE_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5046/api";

async function getHeaders() {
  const session = await getSession();
  
  const headers: HeadersInit = {
    "Content-Type": "application/json",
  };

  if (session?.accessToken) {
    headers["Authorization"] = `Bearer ${session.accessToken}`;
  }

  return headers;
}

export const apiClient = {
  // Método GET genérico
  get: async <T>(endpoint: string): Promise<T> => {
    const headers = await getHeaders();
    
    const res = await fetch(`${BASE_URL}${endpoint}`, {
      method: "GET",
      headers,
    });

    if (!res.ok) {
      throw new Error(`Erro na API: ${res.status} ${res.statusText}`);
    }

    return res.json();
  },

  // Método POST genérico
  post: async <T>(endpoint: string, body: unknown): Promise<T> => {
    const headers = await getHeaders();

    const res = await fetch(`${BASE_URL}${endpoint}`, {
      method: "POST",
      headers,
      body: JSON.stringify(body),
    });

    if (!res.ok) throw new Error(`Erro na API: ${res.status}`);
    return res.json();
  },
  
 // 🟢 MÉTODO PUT TOTALMENTE PROTEGIDO CONTRA CORPO VAZIO
  put: async <T>(endpoint: string, body: unknown): Promise<T> => {
    const headers = await getHeaders();

    const res = await fetch(`${BASE_URL}${endpoint}`, {
      method: "PUT",
      headers,
      body: JSON.stringify(body),
    });

    if (!res.ok) throw new Error(`Erro na API: ${res.status}`);
    
    // 🔍 SE A RESPOSTA VIER VAZIA (Sinal de sucesso no .NET), RETORNA ANTES DE RODAR O .json()
    if (res.status === 204 || res.headers.get("content-length") === "0") {
      return {} as T;
    }
    
    try {
      return await res.json();
    } catch {
      return {} as T; // Garantia final contra strings vazias
    }
  },

  // 🟢 ADICIONADO: Método DELETE para Exclusão de dados
  delete: async <T = void>(endpoint: string): Promise<T> => {
    const headers = await getHeaders();

    const res = await fetch(`${BASE_URL}${endpoint}`, {
      method: "DELETE",
      headers,
    });

    if (!res.ok) throw new Error(`Erro na API: ${res.status}`);
    
    // Se a API responder 204 No Content (comum no .NET para Delete bem-sucedido), 
    // retorna um objeto vazio para não quebrar o .json()
    if (res.status === 204) return {} as T;
    return res.json();
  },
};