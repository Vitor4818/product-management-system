"use client";

import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  Cell
} from "recharts";
import { CategoryBreakdown } from "@/services/dashboard-service";

interface CategoryBarChartProps {
  data: CategoryBreakdown[];
}

export function CategoryBarChart({ data }: CategoryBarChartProps) {
  // Se não tiver dados, mostra mensagem
  if (!data || data.length === 0) {
    return (
      <div className="h-[300px] flex items-center justify-center text-text-muted">
        Sem dados para exibir
      </div>
    );
  }

  return (
    <div className="h-[300px] w-full">
      <ResponsiveContainer width="100%" height="100%">
        <BarChart
          data={data}
          margin={{ top: 20, right: 30, left: 0, bottom: 5 }}
        >
          {/* Grade de fundo pontilhada e sutil */}
          <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#E5E7EB" />
          
          {/* Eixo X: Nomes das categorias */}
          <XAxis 
            dataKey="categoryName" 
            axisLine={false}
            tickLine={false}
            tick={{ fill: '#6B7280', fontSize: 12 }}
            dy={10}
          />
          
          {/* Eixo Y: Quantidades */}
          <YAxis 
            axisLine={false}
            tickLine={false}
            tick={{ fill: '#6B7280', fontSize: 12 }}
          />
          
          {/* Tooltip personalizado ao passar o mouse */}
          <Tooltip 
            cursor={{ fill: 'transparent' }}
            contentStyle={{ 
              backgroundColor: '#FFFFFF', 
              borderRadius: '8px', 
              border: '1px solid #EEF0F6',
              boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)'
            }}
          />

          {/* As Barras */}
          <Bar 
            dataKey="productCount" 
            name="Produtos"
            radius={[4, 4, 0, 0]} // Arredonda topo da barra
            barSize={40} // Largura da barra
          >
            {/* Mágica para usar a cor roxa do ShopSense */}
            {data.map((entry, index) => (
              <Cell key={`cell-${index}`} fill="#5B5CF6" />
            ))}
          </Bar>
        </BarChart>
      </ResponsiveContainer>
    </div>
  );
}