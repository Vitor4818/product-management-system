"use client";

import { useState } from "react";
import { useProducts } from "@/hooks/use-products";
import { useCategoryBreakdown } from "@/hooks/use-category-breakdown";

import { Card, CardContent } from "@/components/ui/card";
import { 
  Search, Grid, List, Plus, LayoutGrid, 
  ChevronLeft, ChevronRight, PackageX, SlidersHorizontal, X, Trash2, Edit3
} from "lucide-react";

export default function ProductsPage() {
  // Estados para Filtros e Paginação
  const [viewMode, setViewMode] = useState<"list" | "grid">("list");
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedCategoryIndex, setSelectedCategoryIndex] = useState("all"); 
  const [page, setPage] = useState(1);
  const pageSize = 8;

  // Estado para controle do Modal Lateral e modo Edição
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingProduct, setEditingProduct] = useState<any | null>(null);
  const [formData, setFormData] = useState({
    Name: "",
    Description: "",
    Price: "",
    StockQuantity: "",
    CategoryName: "", 
  });
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Hooks de Dados e CRUD Completo do hook useProducts
  const { data: categories, isLoading: isLoadingCats } = useCategoryBreakdown();
  const { 
    products: allProducts, 
    isLoading: isLoadingProducts, 
    createProduct, 
    updateProduct, 
    deleteProduct 
  } = useProducts();

  // 1. FILTRAGEM EM MEMÓRIA
  const filteredProducts = (allProducts || []).filter((product) => {
    const name = String(product?.name || product?.Name || "").toLowerCase();
    const description = String(product?.description || product?.Description || "").toLowerCase();
    const search = searchTerm.toLowerCase();

    const matchesSearch = name.includes(search) || description.includes(search);
    
    let matchesCategory = true;
    if (selectedCategoryIndex !== "all" && categories) {
      const targetCategory = categories[Number(selectedCategoryIndex)];
      const targetName = String(targetCategory?.categoryName || targetCategory?.CategoryName || "").toLowerCase();
      const prodCatName = String(product?.categoryName || product?.CategoryName || "").toLowerCase();
      const prodCatId = String(product?.categoryId || product?.CategoryId || "");
      
      if (prodCatName) {
        matchesCategory = prodCatName === targetName;
      } else {
        if (targetName.includes("brinqued")) {
          matchesCategory = prodCatId === "6a40ade858159b1c0b7dda9f" || prodCatId === "6a40ad6158159b1c0b7dda9c";
        } else if (targetName.includes("vestu") || targetName.includes("roup") || targetName.includes("preta")) {
          matchesCategory = prodCatId === "6a40adee58159b1c0b7ddaa0" || prodCatId === "6a40ad6858159b1c0b7dda9d";
        } else {
          matchesCategory = prodCatId === "6a40ade158159b1c0b7dda9e" || prodCatId === "6a40ad2758159b1c0b7dda9b";
        }
      }
    }

    return matchesSearch && matchesCategory;
  });

  // 2. PAGINAÇÃO EM MEMÓRIA
  const totalPages = Math.ceil(filteredProducts.length / pageSize) || 1;
  const products = filteredProducts.slice((page - 1) * pageSize, page * pageSize);

  const isLoading = isLoadingProducts || isLoadingCats;

  // FUNÇÃO PARA ABRIR O MODAL NO MODO CRIAÇÃO
  const handleOpenCreateModal = () => {
    setEditingProduct(null);
    setFormData({ Name: "", Description: "", Price: "", StockQuantity: "", CategoryName: "" });
    setIsModalOpen(true);
  };

  // FUNÇÃO PARA ABRIR O MODAL NO MODO EDIÇÃO
  const handleOpenEditModal = (product: any) => {
    setEditingProduct(product);
    setFormData({
      Name: product.name || product.Name || "",
      Description: product.description || product.Description || "",
      Price: String(product.price ?? product.Price ?? ""),
      StockQuantity: String(product.stockQuantity ?? product.StockQuantity ?? ""),
      CategoryName: product.categoryName || product.CategoryName || "",
    });
    setIsModalOpen(true);
  };

  // FUNÇÃO PARA EXCLUSÃO DE PRODUTO
  const handleDeleteProduct = async (id: string, name: string) => {
    if (confirm(`Tem certeza que deseja excluir o produto "${name}"?`)) {
      try {
        await deleteProduct(id);
      } catch (err) {
        console.error("Erro ao deletar produto:", err);
      }
    }
  };

  // HANDLER DO SUBMIT DO FORMULÁRIO (CREATE & UPDATE)
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.Name || !formData.Price || !formData.StockQuantity || !formData.CategoryName) {
      alert("Por favor, preencha todos os campos obrigatórios.");
      return;
    }

    setIsSubmitting(true);
    try {
      let derivedCategoryId = "";
      const matchedProduct = allProducts?.find(
        p => String(p?.categoryName || p?.CategoryName || "").toLowerCase() === formData.CategoryName.toLowerCase() ||
             String(p?.categoryId || p?.CategoryId || "").toLowerCase() === formData.CategoryName.toLowerCase()
      );
      
      if (matchedProduct) {
        derivedCategoryId = matchedProduct.categoryId || (matchedProduct as any).CategoryId;
      } else {
        derivedCategoryId = formData.CategoryName === "Brinquedos" 
          ? "6a40ad6158159b1c0b7dda9c" 
          : formData.CategoryName === "Roupas" || formData.CategoryName === "Vestuário"
          ? "6a40ad6858159b1c0b7dda9d"
          : "6a40ad2758159b1c0b7dda9b";
      }

      // Payload com tipagem estrita e inclusão do ID para atualização no .NET
      const productPayload: any = {
        Name: formData.Name,
        Description: formData.Description,
        Price: Number(formData.Price) || 0,
        StockQuantity: parseInt(formData.StockQuantity, 10) || 0,
        CategoryId: derivedCategoryId,
        CategoryName: formData.CategoryName
      };

      if (editingProduct) {
        // MODO EDIÇÃO: Alinha o ID da rota com o ID esperado no corpo do DTO C#
        const productId = editingProduct.id || editingProduct.Id;
        productPayload.id = productId;
        productPayload.Id = productId;

        await updateProduct({ id: productId, data: productPayload });
      } else {
        // MODO CRIAÇÃO
        await createProduct(productPayload);
      }

      setFormData({ Name: "", Description: "", Price: "", StockQuantity: "", CategoryName: "" });
      setIsModalOpen(false);
      setEditingProduct(null);
    } catch (err) {
      console.error("Erro ao salvar produto:", err);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="p-8 max-w-7xl mx-auto space-y-6 relative overflow-x-hidden">
      
      {/* HEADER */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold text-zinc-50">Produtos</h1>
          <p className="text-sm text-zinc-400">Gerencie o catálogo, preços e níveis de estoque.</p>
        </div>
        <button 
          onClick={handleOpenCreateModal}
          className="flex items-center gap-2 px-4 py-2 bg-emerald-600 hover:bg-emerald-500 text-white rounded-lg font-medium transition-all text-sm self-start sm:self-auto shadow-lg shadow-emerald-900/20"
        >
          <Plus size={16} /> Novo Produto
        </button>
      </div>

      {/* FILTROS E CONTROLES */}
      <div className="flex flex-col md:flex-row gap-4 justify-between items-stretch md:items-center bg-zinc-900/40 p-4 rounded-xl border border-zinc-800">
        <div className="flex flex-col sm:flex-row gap-3 flex-1">
          <div className="relative flex-1">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-zinc-500" size={18} />
            <input
              type="text"
              placeholder="Buscar por nome ou descrição..."
              value={searchTerm}
              onChange={(e) => { setSearchTerm(e.target.value); setPage(1); }}
              className="w-full bg-zinc-950 border border-zinc-800 rounded-lg pl-10 pr-4 py-2 text-sm text-zinc-200 placeholder-zinc-500 focus:outline-none focus:border-emerald-500 transition-all"
            />
          </div>

          <div className="relative">
            <select
              value={selectedCategoryIndex}
              onChange={(e) => { setSelectedCategoryIndex(e.target.value); setPage(1); }}
              className="bg-zinc-950 border border-zinc-800 rounded-lg px-4 py-2 text-sm text-zinc-300 focus:outline-none focus:border-emerald-500 transition-all appearance-none pr-8 min-w-[160px]"
            >
              <option value="all">Todas Categorias</option>
              {categories?.map((cat, index) => {
                const categoryName = cat?.categoryName || (cat as any)?.CategoryName || `Categoria ${index + 1}`;
                return (
                  <option key={index} value={index.toString()}>
                    {categoryName} ({cat?.productCount || (cat as any)?.productCount || 0})
                  </option>
                );
              })}
            </select>
            <SlidersHorizontal size={14} className="absolute right-3 top-1/2 -translate-y-1/2 text-zinc-500 pointer-events-none" />
          </div>
        </div>

        <div className="flex items-center gap-2 border border-zinc-800 p-1 rounded-lg bg-zinc-950 self-end md:self-auto">
          <button
            onClick={() => setViewMode("list")}
            className={`p-1.5 rounded-md transition-all ${viewMode === "list" ? "bg-zinc-800 text-emerald-500" : "text-zinc-500 hover:text-zinc-300"}`}
          >
            <List size={18} />
          </button>
          <button
            onClick={() => setViewMode("grid")}
            className={`p-1.5 rounded-md transition-all ${viewMode === "grid" ? "bg-zinc-800 text-emerald-500" : "text-zinc-500 hover:text-zinc-300"}`}
          >
            <LayoutGrid size={18} />
          </button>
        </div>
      </div>

      {/* CONTEÚDO PRINCIPAL */}
      {isLoading ? (
        <LoadingSkeleton viewMode={viewMode} />
      ) : products.length === 0 ? (
        <div className="flex flex-col items-center justify-center py-16 text-center bg-zinc-900/20 rounded-2xl border border-zinc-800 border-dashed">
          <PackageX size={48} className="text-zinc-600 mb-3" />
          <h3 className="text-lg font-medium text-zinc-300">Nenhum produto encontrado</h3>
          <p className="text-sm text-zinc-500 max-w-xs mt-1">Tente ajustar seus filtros ou cadastre um novo item.</p>
        </div>
      ) : viewMode === "list" ? (
        /* VISUALIZAÇÃO EM TABELA */
        <div className="overflow-x-auto rounded-xl border border-zinc-800 bg-zinc-900/20">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="border-b border-zinc-800 bg-zinc-900/50 text-xs font-semibold text-zinc-400 uppercase tracking-wider">
                <th className="p-4">Produto</th>
                <th className="p-4">Preço</th>
                <th className="p-4">Estoque</th>
                <th className="p-4 text-center w-28">Ações</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-zinc-800/60 text-sm text-zinc-300">
              {products.map((product) => {
                const idStr = product.id || product.Id || Math.random().toString();
                const productName = product.name || product.Name || "Produto sem nome";
                const productDesc = product.description || product.Description || "";
                const productPrice = product.price ?? product.Price ?? 0;
                const stockQty = product.stockQuantity ?? product.StockQuantity ?? 0;

                return (
                  <tr key={idStr} className="hover:bg-zinc-900/40 transition-colors">
                    <td className="p-4">
                      <div className="font-medium text-zinc-100">{productName}</div>
                      <div className="text-xs text-zinc-500 max-w-md truncate">{productDesc}</div>
                    </td>
                    <td className="p-4 font-mono text-zinc-200">
                      R$ {productPrice.toFixed(2).replace(".", ",")}
                    </td>
                    <td className="p-4">
                      <span className={`inline-flex items-center px-2 py-1 rounded-md text-xs font-medium font-mono ${
                        stockQty <= 0 
                          ? "bg-red-500/10 text-red-400 border border-red-500/20" 
                          : stockQty <= 5 
                          ? "bg-amber-500/10 text-amber-400 border border-amber-500/20" 
                          : "bg-emerald-500/10 text-emerald-400 border border-emerald-500/20"
                      }`}>
                        {stockQty} un
                      </span>
                    </td>
                    <td className="p-4 text-center">
                      <div className="flex items-center justify-center gap-3">
                        <button 
                          onClick={() => handleOpenEditModal(product)}
                          className="p-1 text-zinc-400 hover:text-emerald-500 transition-colors"
                          title="Editar"
                        >
                          <Edit3 size={16} />
                        </button>
                        <button 
                          onClick={() => handleDeleteProduct(idStr, productName)}
                          className="p-1 text-zinc-400 hover:text-red-400 transition-colors"
                          title="Excluir"
                        >
                          <Trash2 size={16} />
                        </button>
                      </div>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      ) : (
        /* VISUALIZAÇÃO EM CARDS */
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
          {products.map((product) => {
            const idStr = product.id || product.Id || Math.random().toString();
            const productName = product.name || product.Name || "Produto sem nome";
            const productDesc = product.description || product.Description || "";
            const productPrice = product.price ?? product.Price ?? 0;
            const stockQty = product.stockQuantity ?? product.StockQuantity ?? 0;

            return (
              <Card key={idStr} className="bg-zinc-900/40 border-zinc-800 shadow-sm hover:border-zinc-700 transition-all">
                <CardContent className="p-5 flex flex-col justify-between h-full space-y-4">
                  <div>
                    <div className="flex justify-between items-start gap-2">
                      <h3 className="font-semibold text-zinc-100 leading-tight truncate">{productName}</h3>
                      <span className={`text-[10px] font-mono font-medium px-2 py-0.5 rounded-full shrink-0 ${
                        stockQty <= 5 ? "bg-red-500/10 text-red-400" : "bg-zinc-800 text-zinc-400"
                      }`}>
                        Estoque: {stockQty}
                      </span>
                    </div>
                    <p className="text-xs text-zinc-500 mt-1 line-clamp-2 h-8">{productDesc}</p>
                  </div>
                  <div className="flex items-center justify-between pt-2 border-t border-zinc-800/60">
                    <span className="text-lg font-bold font-mono text-emerald-400">
                      R$ {productPrice.toFixed(2).replace(".", ",")}
                    </span>
                    <div className="flex items-center gap-2">
                      <button 
                        onClick={() => handleOpenEditModal(product)}
                        className="p-1.5 bg-zinc-800 hover:bg-zinc-700 text-zinc-300 rounded-md transition-all"
                        title="Editar"
                      >
                        <Edit3 size={14} />
                      </button>
                      <button 
                        onClick={() => handleDeleteProduct(idStr, productName)}
                        className="p-1.5 bg-zinc-800/60 hover:bg-red-950 text-red-400 border border-transparent hover:border-red-900 rounded-md transition-all"
                        title="Excluir"
                      >
                        <Trash2 size={14} />
                      </button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            );
          })}
        </div>
      )}

      {/* PAGINAÇÃO */}
      {filteredProducts.length > 0 && (
        <div className="flex items-center justify-between pt-4 border-t border-zinc-900">
          <p className="text-xs text-zinc-500">
            Página <span className="text-zinc-300 font-medium">{page}</span> de <span className="text-zinc-300 font-medium">{totalPages}</span> 
            <span className="ml-2 text-zinc-600">({filteredProducts.length} produtos no total)</span>
          </p>
          <div className="flex items-center gap-2">
            <button
              onClick={() => setPage((p) => Math.max(p - 1, 1))}
              disabled={page === 1}
              className="p-2 border border-zinc-800 rounded-lg text-zinc-400 hover:text-zinc-200 disabled:opacity-40 disabled:hover:text-zinc-400 transition-all bg-zinc-950"
            >
              <ChevronLeft size={16} />
            </button>
            <button
              onClick={() => setPage((p) => Math.min(p + 1, totalPages))}
              disabled={page === totalPages}
              className="p-2 border border-zinc-800 rounded-lg text-zinc-400 hover:text-zinc-200 disabled:opacity-40 disabled:hover:text-zinc-400 transition-all bg-zinc-950"
            >
              <ChevronRight size={16} />
            </button>
          </div>
        </div>
      )}

      {/* --- MODAL LATERAL (GAVETA / SHEET) --- */}
      <div 
        onClick={() => setIsModalOpen(false)}
        className={`fixed inset-0 bg-black/60 backdrop-blur-sm z-50 transition-opacity duration-300 ${
          isModalOpen ? "opacity-100 pointer-events-auto" : "opacity-0 pointer-events-none"
        }`} 
      />

      <div className={`fixed top-0 right-0 bottom-0 w-full sm:max-w-md bg-zinc-950 border-l border-zinc-800 z-50 p-6 shadow-2xl flex flex-col justify-between transform transition-transform duration-300 h-full ${
        isModalOpen ? "translate-x-0" : "translate-x-full"
      }`}>
        <div>
          {/* Header do Modal Dinâmico */}
          <div className="flex items-center justify-between pb-5 border-b border-zinc-800">
            <div>
              <h2 className="text-xl font-bold text-zinc-100">
                {editingProduct ? "Editar Produto" : "Cadastrar Produto"}
              </h2>
              <p className="text-xs text-zinc-400 mt-0.5">
                {editingProduct ? "Altere as informações necessárias do item." : "Insira os dados para expandir o inventário."}
              </p>
            </div>
            <button 
              onClick={() => setIsModalOpen(false)}
              className="p-1.5 hover:bg-zinc-900 rounded-md text-zinc-400 hover:text-zinc-200 transition-all"
            >
              <X size={18} />
            </button>
          </div>

          {/* Formulário */}
          <form id="product-form" onSubmit={handleSubmit} className="space-y-4 pt-5">
            <div>
              <label className="block text-xs font-medium text-zinc-400 mb-1.5">Nome do Produto *</label>
              <input
                type="text"
                required
                placeholder="Ex: Camiseta Oversized Branca"
                value={formData.Name}
                onChange={(e) => setFormData({ ...formData, Name: e.target.value })}
                className="w-full bg-zinc-900 border border-zinc-800 rounded-lg px-3 py-2 text-sm text-zinc-200 focus:outline-none focus:border-emerald-500 transition-all"
              />
            </div>

            <div>
              <label className="block text-xs font-medium text-zinc-400 mb-1.5">Descrição (Opcional)</label>
              <textarea
                placeholder="Detalhes sobre material, especificações..."
                value={formData.Description}
                onChange={(e) => setFormData({ ...formData, Description: e.target.value })}
                rows={3}
                className="w-full bg-zinc-900 border border-zinc-800 rounded-lg px-3 py-2 text-sm text-zinc-200 focus:outline-none focus:border-emerald-500 transition-all resize-none"
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-xs font-medium text-zinc-400 mb-1.5">Preço (R$) *</label>
                <input
                  type="number"
                  step="0.01"
                  required
                  placeholder="0,00"
                  value={formData.Price}
                  onChange={(e) => setFormData({ ...formData, Price: e.target.value })}
                  className="w-full bg-zinc-900 border border-zinc-800 rounded-lg px-3 py-2 text-sm text-zinc-200 focus:outline-none focus:border-emerald-500 transition-all font-mono"
                />
              </div>

              <div>
                <label className="block text-xs font-medium text-zinc-400 mb-1.5">Quantidade em Estoque *</label>
                <input
                  type="number"
                  required
                  placeholder="0"
                  value={formData.StockQuantity}
                  onChange={(e) => setFormData({ ...formData, StockQuantity: e.target.value })}
                  className="w-full bg-zinc-900 border border-zinc-800 rounded-lg px-3 py-2 text-sm text-zinc-200 focus:outline-none focus:border-emerald-500 transition-all font-mono"
                />
              </div>
            </div>

            <div>
              <label className="block text-xs font-medium text-zinc-400 mb-1.5">Categoria *</label>
              <div className="relative">
                <select
                  required
                  value={formData.CategoryName}
                  onChange={(e) => setFormData({ ...formData, CategoryName: e.target.value })}
                  className="w-full bg-zinc-900 border border-zinc-800 rounded-lg px-3 py-2 text-sm text-zinc-300 focus:outline-none focus:border-emerald-500 transition-all appearance-none pr-8"
                >
                  <option value="" disabled>Selecione uma categoria</option>
                  {categories?.map((cat, index) => {
                    const name = cat?.categoryName || (cat as any)?.CategoryName || "";
                    if (!name) return null;
                    return (
                      <option key={index} value={name}>
                        {name}
                      </option>
                    );
                  })}
                </select>
                <SlidersHorizontal size={14} className="absolute right-3 top-1/2 -translate-y-1/2 text-zinc-500 pointer-events-none" />
              </div>
            </div>
          </form>
        </div>

        {/* Footer com Ações */}
        <div className="border-t border-zinc-800 pt-4 flex items-center gap-3">
          <button
            type="button"
            onClick={() => setIsModalOpen(false)}
            className="flex-1 px-4 py-2 bg-zinc-900 hover:bg-zinc-800 border border-zinc-800 text-zinc-300 rounded-lg text-sm font-medium transition-all"
          >
            Cancelar
          </button>
          <button
            type="submit"
            form="product-form"
            disabled={isSubmitting}
            className="flex-1 px-4 py-2 bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 disabled:hover:bg-emerald-600 text-white rounded-lg text-sm font-medium transition-all shadow-lg shadow-emerald-900/10"
          >
            {isSubmitting ? "Salvando..." : "Salvar Alterações"}
          </button>
        </div>
      </div>

    </div>
  );
}

function LoadingSkeleton({ viewMode }: { viewMode: "list" | "grid" }) {
  return viewMode === "list" ? (
    <div className="w-full border border-zinc-800 rounded-xl bg-zinc-900/10 animate-pulse divide-y divide-zinc-800">
      {[...Array(5)].map((_, i) => (
        <div key={i} className="p-5 flex justify-between items-center">
          <div className="space-y-2 flex-1"><div className="h-4 bg-zinc-800 rounded w-1/4"></div><div className="h-3 bg-zinc-800 rounded w-1/2"></div></div>
          <div className="h-4 bg-zinc-800 rounded w-16 mx-4"></div>
          <div className="h-6 bg-zinc-800 rounded w-14"></div>
        </div>
      ))}
    </div>
  ) : (
    <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4 animate-pulse">
      {[...Array(4)].map((_, i) => (
        <div key={i} className="h-40 border border-zinc-800 bg-zinc-900/10 rounded-xl p-5 flex flex-col justify-between">
          <div className="space-y-2"><div className="h-4 bg-zinc-800 rounded w-3/4"></div><div className="h-3 bg-zinc-800 rounded w-full"></div></div>
          <div className="h-8 bg-zinc-800 rounded w-full mt-4"></div>
        </div>
      ))}
    </div>
  );
}