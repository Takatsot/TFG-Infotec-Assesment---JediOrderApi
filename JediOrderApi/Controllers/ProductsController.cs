using AutoMapper;
using Core.Interfaces;
using Core.Models.Domain;
using Core.Models.DTO;
using Core.Specification;
using JediOrderApi.CustomActionFilters;
using JediOrderApi.RequestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JediOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _genericRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all products.
        /// GET:/api/products?filterOn=Name&filterQuery=Coca&sortBy=name&isAscending=true&pageNumber=1&pageSize=10
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetAllAsync([FromQuery] ProductSpecParams productSpecParams)
        {
            var spec = new ProductSpecification(productSpecParams);
            var pagination = await CreatePageResult(_genericRepository, spec, productSpecParams.PageIndex, productSpecParams.PageSize);
            return Ok(_mapper.Map<IReadOnlyList<ProductsResponse>>(pagination));
        }

        /// <summary>
        /// Get single product by Id.
        /// GET: https://localhost:portnumber/api/products/{id}
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductByIdAsync([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Invalid product id.");

            var product = await _genericRepository.GetByIdAsync(id);
            if (product == null) return NotFound(new { Message = "Product not found." });

            return Ok(_mapper.Map<ProductsResponse>(product));
        }

        /// <summary>
        /// Creates a product.
        /// POST: https://localhost:portnumber/api/products
        /// </summary>
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<Product>> CreateAsync([FromForm] AddProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            _genericRepository.Add(product);
            if (await _genericRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetProductByIdAsync), new { id = product.Id }, _mapper.Map<ProductsResponse>(product));
            }
            return BadRequest("Problem creating product");
        }

        /// <summary>
        /// Update a product.
        /// PUT: https://localhost:portnumber/api/products/{id}
        /// </summary>
        [HttpPut("{id:int}")]
        [ValidateModel]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] AddProductRequest request)
        {
            if (id <= 0 || request == null) return BadRequest("Invalid parameters.");

            var product = _mapper.Map<Product>(request);
            _genericRepository.Update(product);

            if (await _genericRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem updating the product");
        }

        /// <summary>
        /// Delete a product.
        /// DELETE: https://localhost:portnumber/api/products/{id}
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Invalid product id.");

            var product = await _genericRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            _genericRepository.Delete(product);

            if (await _genericRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem deleting product");
        }

        #region Validations
        private void ValidateFileUpload(ImageUploadRequest request)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(request?.File?.FileName);

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }
            else if (request?.File?.Length > 10 * 1024 * 1024) // 10MB limit
            {
                ModelState.AddModelError("file", "File size exceeds 10MB.");
            }
        }
        #endregion
    }
}
