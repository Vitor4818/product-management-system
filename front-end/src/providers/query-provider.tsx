"use client";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { useState } from "react";

export function QueryProvider({ children }: { children: React.ReactNode }) {
  // O QueryClient deve ser criado dentro do componente para evitar
  // compartilhamento de cache entre requisições no Server Side (Next.js quirk)
  const [queryClient] = useState(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            // Se o usuário mudar de aba e voltar, não refaz o fetch imediatamente
            refetchOnWindowFocus: false, 
            // Tenta 1 vez novamente se der erro, depois falha
            retry: 1,
            // Dados considerados "frescos" por 1 minuto
            staleTime: 1000 * 60, 
          },
        },
      })
  );

  return (
    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  );
}