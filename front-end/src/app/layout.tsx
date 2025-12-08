import type { Metadata } from "next";
import { Inter, Poppins } from "next/font/google";
import "./globals.css";
import { Providers } from "./providers";
import { QueryProvider } from "@/providers/query-provider";
import { SideBar } from "@/components/sidebar/sidebar";

// Configura a fonte Inter (Texto padrão)
const inter = Inter({
  subsets: ["latin"],
  variable: "--font-inter",
  display: "swap",
});

// Configura a fonte Poppins (Títulos)
const poppins = Poppins({
  weight: ["400", "500", "600", "700"],
  subsets: ["latin"],
  variable: "--font-poppins", // O CSS vai ler essa variável
  display: "swap",
});

export const metadata: Metadata = {
  title: "ShopSense Admin",
  description: "Sistema de Gerenciamento",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="pt-br">
      <body className="... flex flex-1">
        <SideBar/>
        <Providers> 
          <QueryProvider>
             {children}
          </QueryProvider>
        </Providers>
      </body>
    </html>
  );
}