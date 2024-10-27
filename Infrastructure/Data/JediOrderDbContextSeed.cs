using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Text.Json;

namespace Infrastructure.Data
{
    public static class JediOrderDbContextSeed
    {
        public static async Task SeedDataAsync(JediOrderDbContext context)
        {


            if (!context.Products.Any())
            {
                //Seed data for 'Products'
                string productsJson = await File.ReadAllTextAsync("Data/JediOrderDataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsJson);

                if (products == null) return;

                context.Products.AddRange(products);

                // Seed data for 'Reviews'
                var reviewsJson = await File.ReadAllTextAsync("Data/JediOrderDataSeed/reviews.json");
                var reviews = JsonSerializer.Deserialize<List<Review>>(reviewsJson);

                if (reviews == null) return;

                context.Products.AddRange(products);

                await context.SaveChangesAsync();
            }

           
        }
    }
}
