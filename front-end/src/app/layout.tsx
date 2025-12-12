import type { Metadata } from "next";
import { Inter, Poppins } from "next/font/google";
import "./globals.css";
import { Providers } from "./providers";
import { QueryProvider } from "@/providers/query-provider";
import { AppSidebar } from "@/components/sidebar/sidebar";
import { SidebarProvider} from "@/components/ui/sidebar";

// Fontes
const inter = Inter({
  subsets: ["latin"],
  variable: "--font-inter",
  display: "swap",
});

const poppins = Poppins({
  weight: ["400", "500", "600", "700"],
  subsets: ["latin"],
  variable: "--font-poppins",
  display: "swap",
});

export const metadata: Metadata = {
  title: "ShopSense Admin",
  description: "Sistema de Gerenciamento",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="pt-br" className={`${inter.variable} ${poppins.variable}`}>
      <body>
        <Providers>
          <QueryProvider>
            <SidebarProvider defaultOpen={true}>
              <AppSidebar />
              <main className="flex-1">
                {children}
              </main>
            </SidebarProvider>
          </QueryProvider>
        </Providers>
      </body>
    </html>
  );
}
