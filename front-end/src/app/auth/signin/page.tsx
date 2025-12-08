"use client";

import { signIn } from "next-auth/react";
import { ShoppingBag, ArrowRight, ShieldCheck, LayoutDashboard } from "lucide-react";
import { Button } from "@/components/ui/button";
import { 
  Card, 
  CardContent, 
  CardFooter, 
  CardHeader, 
  CardTitle, 
  CardDescription 
} from "@/components/ui/card";

export default function SignInPage() {
  
  const handleLogin = () => {
    signIn("keycloak", { callbackUrl: "/dashboard" });
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-background-main p-4">
      
      <Card className="w-full max-w-[400px] shadow-card border-border-soft animate-in fade-in zoom-in duration-300">
        
        <CardHeader className="text-center pb-2">
          <div className="mx-auto mb-4 inline-flex items-center justify-center w-14 h-14 rounded-xl bg-primary-100 text-primary-700 shadow-sm">
            <ShoppingBag size={28} strokeWidth={2.5} />
          </div>
          <CardTitle className="text-2xl font-title font-semibold text-primary-700">
            ShopSense
          </CardTitle>
          <CardDescription>
            Painel administrativo de gestão.
          </CardDescription>
        </CardHeader>

        <CardContent className="space-y-4 pt-4">
          <Button 
            onClick={handleLogin}
            className="w-full h-12 text-md font-medium gap-2 shadow-md hover:shadow-lg transition-all"
            size="lg"
          >
            <LayoutDashboard size={18} />
            Entrar com SSO
            <ArrowRight size={18} className="ml-1 opacity-70" />
          </Button>
        </CardContent>

        <CardFooter className="flex flex-col gap-4 border-t border-border-soft bg-background-light/50 pt-6">
          <div className="flex items-center gap-2 text-xs text-muted-foreground">
            <ShieldCheck size={14} className="text-status-success" />
            <span>Ambiente Protegido via Keycloak</span>
          </div>
        </CardFooter>
      </Card>
      
      <div className="fixed bottom-6 text-center w-full text-xs text-muted-foreground opacity-50 font-mono">
        © 2025 ShopSense System v1.0
      </div>
    </div>
  );
}