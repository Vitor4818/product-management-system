"use client"; 

import { useCurrentUser } from "@/hooks/use-current-user"; 
import { signIn, signOut } from "next-auth/react";

export default function UserProfile() {
  const { user, isAuthenticated, isLoading } = useCurrentUser();

  if (isLoading) return <p>Carregando...</p>;

  if (isAuthenticated) {
    return (
      <div className="p-4 border rounded shadow">
        <p className="font-bold">Bem-vindo, {user?.name}</p>
        <p className="text-sm text-gray-500">{user?.email}</p>
        <button 
          onClick={() => signOut()}
          className="mt-2 bg-red-500 text-white px-4 py-2 rounded"
        >
          Sair
        </button>
      </div>
    );
  }

  return (
    <button 
      onClick={() => signIn("keycloak")}
      className="bg-blue-500 text-white px-4 py-2 rounded"
    >
      Login com Keycloak
    </button>
  );
}