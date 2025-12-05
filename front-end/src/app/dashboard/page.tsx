import { getServerSession } from "next-auth";
import { authOptions } from "@/lib/auth-options";

export default async function DashboardPage() {
  const session = await getServerSession(authOptions);

  return (
    <div className="p-10">
      <h1 className="text-3xl font-bold mb-4">🔒 Área Restrita (Dashboard)</h1>
      
      <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded mb-4">
        <p className="font-bold">Acesso Autorizado pelo Middleware!</p>
        <p>Olá, {session?.user?.name}.</p>
      </div>

      <div className="bg-slate-900 text-slate-50 p-6 rounded-lg overflow-auto">
        <h2 className="text-xl font-bold mb-2 text-yellow-400">Dados da Sessão:</h2>
        <pre className="text-xs font-mono">
          {JSON.stringify(session, null, 2)}
        </pre>
      </div>
    </div>
  );
}