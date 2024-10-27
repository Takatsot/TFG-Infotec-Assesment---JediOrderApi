
using Core.Models.Domain;
using Infrastructure.Data;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repository
{
    public class ProductsRepository : IProductRepository
    {
        private readonly JediOrderDbContext _dbContext;

        public ProductsRepository(JediOrderDbContext jediOrderDbContext)
        {
            _dbContext = jediOrderDbContext;
        }

        #region Get Methods
        public async Task<List<Product>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100)
        {
            IQueryable<Product> query = _dbContext.Products.AsQueryable();

            // filtering based on parameters
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => x.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("Type", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(x => x.Type.Contains(filterQuery));
                }
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isAscending ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    query = isAscending ? query.OrderBy(x => x.Price) : query.OrderByDescending(x => x.Price);
                }
            }

            //Pagination
            var skipResullt = (pageNumber - 1) * pageSize;

            // Execute the query and return the result as a list
            return await query.Skip(skipResullt).Take(pageSize).ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        }
        #endregion

        #region Modify Methods
        public async Task<Product> CreateAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (existingProduct == null)
            {
                return null;
            }

            GetUpdatedProductEntity(existingProduct, product);
            await _dbContext.SaveChangesAsync();

            return existingProduct;
        }

        public async Task<Product?> DeleteAsync(int id)
        {
            Product? existingProduct = await GetProductEntityAsync(id);

            if (existingProduct == null)
            {
                return null;
            }

            _dbContext.Products.Remove(existingProduct);
            await _dbContext.SaveChangesAsync();
            return existingProduct;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// returns an  product entity.
        /// </summary>
        private void GetUpdatedProductEntity(Product existingProduct, Product product)
        {
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.PictureUrl = product.PictureUrl;
            existingProduct.Type = product.Type;
            existingProduct.DateModified = DateTime.Now;
            existingProduct.UserModified = product.UserCreated;
        }

        /// <summary>
        /// Returns the product entity if found; otherwise, null.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product entity or null if not found.</returns>
        private async Task<Product?> GetProductEntityAsync(int id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        #endregion
    }
}
