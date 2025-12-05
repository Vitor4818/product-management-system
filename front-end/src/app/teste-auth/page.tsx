"use client";

import { useSession, signIn, signOut } from "next-auth/react";

export default function TestAuthPage() {
  const { data: session, status } = useSession();

  if (status === "loading") {
    return <p>Carregando sessão...</p>;
  }

  if (status === "unauthenticated") {
    return (
      <div className="p-10">
        <h1 className="text-xl font-bold mb-4">Você não está logado</h1>
        <button 
          onClick={() => signIn("keycloak")}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          Entrar com Keycloak
        </button>
      </div>
    );
  }

  return (
    <div className="p-10 break-all">
      <h1 className="text-xl font-bold text-green-600 mb-4">Usuário Autenticado!</h1>
      
      <div className="mb-4">
        <p><strong>Nome:</strong> {session?.user?.name}</p>
        <p><strong>Email:</strong> {session?.user?.email}</p>
        <p><strong>ID (Keycloak Subject):</strong> {session?.user?.id}</p>
      </div>

      <div className="bg-gray-100 p-4 rounded mb-4 border border-gray-300">
        <h3 className="font-bold mb-2">Access Token (Para usar na API .NET/Java):</h3>
        <code className="text-xs text-red-600 font-mono">
          {session?.accessToken || "Token não encontrado na sessão"}
        </code>
      </div>

      <div className="bg-gray-900 text-green-400 p-4 rounded mb-4 text-xs font-mono">
        <h3 className="font-bold text-white mb-2">Dump da Sessão Completa:</h3>
        <pre>{JSON.stringify(session, null, 2)}</pre>
      </div>

      <button 
        onClick={() => signOut()}
        className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700"
      >
        Sair (Logout)
      </button>
    </div>
  );
}