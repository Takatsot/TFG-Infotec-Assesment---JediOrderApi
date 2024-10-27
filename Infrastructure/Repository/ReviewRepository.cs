
using Core.Models.Domain;
using Infrastructure.Data;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly JediOrderDbContext _dbContext;

        public ReviewRepository(JediOrderDbContext jediOrderDbContext)
        {
            _dbContext = jediOrderDbContext;
        }

        #region Get Methods
        public async Task<List<Review>> GetAllAsync()
        {
            return await _dbContext.Reviews.Include("Products").ToListAsync();
        }
        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _dbContext.Reviews.Include("Products").FirstOrDefaultAsync(x => x.Id == id);
        }
        #endregion

        #region Modify Methods
        public async Task<Review> CreateAsync(Review review)
        {
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> UpdateAsync(int id, Review review)
        {
            Review? existingReviews = await _dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == id);

            if (existingReviews == null)
            {
                return null;
            }

            GetUpdatedEntity(existingReviews, review);
            await _dbContext.SaveChangesAsync();

            return existingReviews;
        }

        public async Task<Review?> DeleteAsync(int id)
        {
            Review? existingReviews = await GetEntityAsync(id);

            if (existingReviews == null)
            {
                return null;
            }

            _dbContext.Reviews.Remove(existingReviews);
            await _dbContext.SaveChangesAsync();
            return existingReviews;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// returns an  product entity.
        /// </summary>
        private void GetUpdatedEntity(Review existingReview, Review review)
        {
            existingReview.ReviewTitle = review.ReviewTitle;
            existingReview.Comment = review.Comment;
            existingReview.DateModified = DateTime.Now;
            existingReview.UserModified = review.UserCreated;
        }

        /// <summary>
        /// Returns the product entity if found; otherwise, null.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product entity or null if not found.</returns>
        private async Task<Review?> GetEntityAsync(int id)
        {
            return await _dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == id);
        }

        #endregion

    }
}
