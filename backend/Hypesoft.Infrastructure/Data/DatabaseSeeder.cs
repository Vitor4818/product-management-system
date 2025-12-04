using Hypesoft.Domain.Entities;
using Hypesoft.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hypesoft.Infrastructure.Services
{
    public static class DatabaseSeeder
    {
        /// <summary>
        /// Popula o banco de dados com dados iniciais se as coleções estiverem vazias.
        /// </summary>
        /// <param name="database">A instância do IMongoDatabase.</param>
        public static async Task SeedDataAsync(IMongoDatabase database)
        {
            var productsCollection = database.GetCollection<Product>("Products");
            var categoriesCollection = database.GetCollection<Category>("Categories");

            // 1. Verificar se já existem produtos
            var productCount = await productsCollection.CountDocumentsAsync(Builders<Product>.Filter.Empty);
            if (productCount > 0)
            {
                Console.WriteLine("--- Banco de Dados já Populado. Pulando Seeding. ---");
                return; // Já populado, sair.
            }

            // 2. Criar as Categorias
            var categories = new List<Category>
            {
                new Category { Id = ObjectId.GenerateNewId().ToString(), Name = "Eletrônicos" },
                new Category { Id = ObjectId.GenerateNewId().ToString(), Name = "Vestuário" },
                new Category { Id = ObjectId.GenerateNewId().ToString(), Name = "Alimentos" }
            };

            await categoriesCollection.InsertManyAsync(categories);
            Console.WriteLine($"[SEED] Adicionadas {categories.Count} Categorias.");

            // 3. Criar os Produtos (Referenciando os IDs das Categorias)
            var prodIdEletronicos = categories[0].Id;
            var prodIdVestuario = categories[1].Id;
            var prodIdAlimentos = categories[2].Id;
            
            var products = new List<Product>
            {
                new Product { Name = "Smartphone X", Description = "Celular de última geração.", Price = 3500.00m, StockQuantity = 150, CategoryId = prodIdEletronicos },
                new Product { Name = "Laptop Pro", Description = "Notebook para desenvolvimento.", Price = 7200.50m, StockQuantity = 50, CategoryId = prodIdEletronicos },
                new Product { Name = "Mouse Gamer", Description = "Mouse óptico de alta precisão.", Price = 250.00m, StockQuantity = 8, CategoryId = prodIdEletronicos }, // Baixo Estoque
                
                new Product { Name = "Camiseta Básica", Description = "Algodão Pima.", Price = 80.00m, StockQuantity = 300, CategoryId = prodIdVestuario },
                new Product { Name = "Calça Jeans", Description = "Corte reto, denim premium.", Price = 199.90m, StockQuantity = 45, CategoryId = prodIdVestuario },
                new Product { Name = "Meias Esportivas", Description = "Kit com 6 pares.", Price = 49.99m, StockQuantity = 5, CategoryId = prodIdVestuario }, // Baixo Estoque
                
                new Product { Name = "Café Gourmet", Description = "Grãos 100% Arábica.", Price = 45.00m, StockQuantity = 90, CategoryId = prodIdAlimentos },
                new Product { Name = "Barra de Cereal", Description = "Caixa com 12 unidades.", Price = 25.00m, StockQuantity = 7, CategoryId = prodIdAlimentos }, // Baixo Estoque
                new Product { Name = "Chocolate Amargo", Description = "70% cacau.", Price = 15.00m, StockQuantity = 120, CategoryId = prodIdAlimentos },
                new Product { Name = "Água Mineral", Description = "Garrafa 500ml.", Price = 2.50m, StockQuantity = 3, CategoryId = prodIdAlimentos } // Baixo Estoque
            };

            await productsCollection.InsertManyAsync(products);
            
            Console.WriteLine("--- Banco de Dados Populado com 3 Categorias e 10 Produtos ---");
        }
    }
}