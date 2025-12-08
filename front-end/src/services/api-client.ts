import { getSession } from "next-auth/react";

// Defina sua URL base aqui (ou use variável de ambiente)
// Se você ainda não tem backend rodando, pode deixar localhost:5000 por enquanto
const BASE_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5046/api";

async function getHeaders() {
  const session = await getSession();
  
  const headers: HeadersInit = {
    "Content-Type": "application/json",
  };

  // Aqui está a mágica: Injeta o Token se o usuário estiver logado
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
      // Tratamento básico de erro (ex: 401 token expirado)
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
  
  // Você pode adicionar put, delete, patch aqui seguindo o mesmo padrão...
};