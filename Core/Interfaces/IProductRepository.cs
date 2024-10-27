using Core.Models.Domain;
namespace Infrastructure.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, 
            bool isAscending = true,int pageNumber = 1,int pageSize = 100);
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<Product?> UpdateAsync(int id, Product product);
        Task<Product?> DeleteAsync(int id);
    }
}
