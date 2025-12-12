// src/app/dashboard/page.tsx
"use client";

import { useMetrics } from "@/hooks/use-metrics"; 
import { useLowStock } from "@/hooks/use-low-stock"; 
import { useCategoryBreakdown } from "@/hooks/use-category-breakdown"; 

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { DollarSign, Package, TrendingUp, AlertTriangle } from "lucide-react";
import { CategoryBarChart } from "@/components/charts/category-bar-chart";


export default function DashboardPage() {
  const { metrics, isLoading: loadingMetrics } = useMetrics();
  const { data: lowStockData, isLoading: loadingLowStock } = useLowStock();
  const { data: breakdown, isLoading: loadingBreakdown } = useCategoryBreakdown();


  if (loadingMetrics || loadingLowStock || loadingBreakdown) {
      return <div className="p-8 text-center">Carregando Dashboard...</div>;
  }

  const lowStockItems = lowStockData?.items || [];
  const totalProducts = metrics?.totalProducts ?? 0;
  const totalStockValue = metrics?.totalStockValue ?? 0;

  return (
    <div className="p-8 max-w-7xl mx-auto space-y-6">
      <h1 className="text-3xl font-bold text-text-primary font-bold">Dashboard</h1>
      
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
          
          <Card className="shadow-sm">
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">
                Total de Produtos
              </CardTitle>
              <Package className="h-4 w-4 text-primary-500" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">
                {totalProducts.toLocaleString()}
              </div>
              <p className="text-xs text-text-muted">+ Produtos no catálogo</p>
            </CardContent>
          </Card>

          <Card className="shadow-sm">
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">
                Valor Total do Estoque
              </CardTitle>
              <DollarSign className="h-4 w-4 text-primary-500" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">
                R$ {totalStockValue.toFixed(2).replace('.', ',')}
              </div>
              <p className="text-xs text-text-muted">Valor de custo total</p>
            </CardContent>
          </Card>
      </div>

      <div className="grid gap-4 lg:grid-cols-7">
        
        <Card className="col-span-4 shadow-sm border-border-soft">
          <CardHeader>
            <CardTitle className="text-lg text-primary-900">Estoque por Categoria</CardTitle>
            <p className="text-sm text-text-muted">Distribuição da quantidade de produtos</p>
          </CardHeader>
          <CardContent className="pl-0">
            {loadingBreakdown ? (
               <div className="h-[300px] flex items-center justify-center">Carregando gráfico...</div>
            ) : (
               <CategoryBarChart data={breakdown || []} />
            )}
          </CardContent>
        </Card>

        <Card className="col-span-3 shadow-sm">
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
                <AlertTriangle className="h-4 w-4 text-red-500" /> 
                Produtos em Alerta
            </CardTitle>
            <p className="text-sm text-text-muted">
                {lowStockData?.totalCount} itens com estoque crítico.
            </p>
          </CardHeader>
          <CardContent className="space-y-3">
            {lowStockItems.length > 0 ? lowStockItems.map((product) => (
              <div key={product.id} className="flex items-center">
                <div className="ml-4 space-y-1">
                  <p className="text-sm font-medium leading-none">{product.name}</p>
                  <p className="text-sm text-text-muted">R$ {product.price.toFixed(2)}</p>
                </div>
                <div className="ml-auto font-medium text-sm text-red-500">
                  {product.stockQuantity} un
                </div>
              </div>
            )) : (
                <p className="text-center text-text-muted py-4">Nenhum produto em alerta. 🎉</p>
            )}
          </CardContent>
        </Card>
        

      </div>
    </div>
  );
}