using AutoMapper;
using Core.Interfaces;
using Core.Models.Domain;
using Core.Models.DTO;
using Infrastructure.Repository.Interfaces;
using JediOrderApi.CustomActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JediOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(IReviewRepository reviewRepository, IMapper mapper, ILogger<ReviewsController> logger)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all reviews.
        /// GET: https://localhost:portnumber/api/reviews
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<Review> reviews = await _reviewRepository.GetAllAsync();
                return Ok(_mapper.Map<List<ReviewResponse>>(reviews));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all reviews.");
                return StatusCode(500, "An error occurred while fetching reviews.");
            }
        }

        /// <summary>
        /// Get a single review by Id.
        /// GET: https://localhost:portnumber/api/reviews/{id}
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            if (id == 0)
            {
                _logger.LogWarning("GetByIdAsync was called with an invalid id: {Id}", id);
                return BadRequest("Invalid ID");
            }

            try
            {
                var review = await _reviewRepository.GetByIdAsync(id);
                if (review == null)
                {
                    return NotFound(new { Message = "Review not found." });
                }
                return Ok(_mapper.Map<ReviewResponse>(review));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching review with ID: {Id}", id);
                return StatusCode(500, "An error occurred while fetching the review.");
            }
        }

        /// <summary>
        /// Creates a review.
        /// POST: https://localhost:portnumber/api/reviews
        /// </summary>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateAsync([FromBody] AddReviewsRequest request)
        {
            try
            {
                Review review = _mapper.Map<Review>(request);
                review = await _reviewRepository.CreateAsync(review);
                ReviewResponse reviewResponse = _mapper.Map<ReviewResponse>(review);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = reviewResponse.ReviewsID }, reviewResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new review.");
                return StatusCode(500, "An error occurred while creating the review.");
            }
        }

        /// <summary>
        /// Updates a review.
        /// PUT: https://localhost:portnumber/api/reviews/{id}
        /// </summary>
        [HttpPut("{id:int}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] AddReviewsRequest request)
        {
            if (id == 0)
            {
                _logger.LogWarning("UpdateAsync was called with an invalid id: {Id}", id);
                return BadRequest("Invalid ID");
            }

            try
            {
                Review review = _mapper.Map<Review>(request);
                Review? updatedReview = await _reviewRepository.UpdateAsync(id, review);

                if (updatedReview == null)
                {
                    return NotFound();
                }

                ReviewResponse reviewResponse = _mapper.Map<ReviewResponse>(updatedReview);
                return Ok(reviewResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating review with ID: {Id}", id);
                return StatusCode(500, "An error occurred while updating the review.");
            }
        }

        /// <summary>
        /// Deletes a review.
        /// DELETE: https://localhost:portnumber/api/reviews/{id}
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            if (id == 0)
            {
                _logger.LogWarning("DeleteAsync was called with an invalid id: {Id}", id);
                return BadRequest("Invalid ID");
            }

            try
            {
                Review? review = await _reviewRepository.DeleteAsync(id);
                if (review == null)
                {
                    return NotFound();
                }

                ReviewResponse reviewResponse = _mapper.Map<ReviewResponse>(review);
                return Ok(reviewResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting review with ID: {Id}", id);
                return StatusCode(500, "An error occurred while deleting the review.");
            }
        }
    }
}
