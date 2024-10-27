using Core.Models.Domain;
namespace Infrastructure.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Products>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, 
            bool isAscending = true,int pageNumber = 1,int pageSize = 100);
        Task<Products?> GetProductByIdAsync(int id);
        Task<Products> CreateAsync(Products product);
        Task<Products?> UpdateAsync(int id, Products product);
        Task<Products?> DeleteAsync(int id);
    }
}
