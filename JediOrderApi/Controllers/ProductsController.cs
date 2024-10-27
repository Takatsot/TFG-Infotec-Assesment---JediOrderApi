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

        public ProductsController(IGenericRepository<Product> genericRepository,IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper             = mapper;
        }

        /// <summary>
        /// Get all products.
        /// GET:/api/products?filterOn=Name&filterQuery=Coca&sortBy=name&isAscending=true&pageNumber=1&pageSize=10
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetAllAsync([FromQuery]ProductSpecParams productSpecParams)
        {
            ProductSpecification spec = new ProductSpecification(productSpecParams);
            var pagination = await CreatePageResult(_genericRepository, spec, productSpecParams.PageIndex, productSpecParams.PageSize);
            return Ok(_mapper.Map<IReadOnlyList<ProductsResponse>>(pagination));
        }

        /// <summary>
        /// Get single product by Id.
        /// GET: https://localhost:portnumber/api/products/{id}
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetProductByIdAsync([FromRoute] int id)
        {
            if (id == 0) return BadRequest();
            var product = await _genericRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new { Message = "Product not found." });
            }

            ProductsResponse productResponse = _mapper.Map<ProductsResponse>(product);
            return Ok(productResponse);
        }

        /// <summary>
        /// Creates a product .
        /// POST: https://localhost:portnumber/api/products
        /// </summary>
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<Product>> CreateAsync([FromForm] AddProductRequest request)
        {

            // Map request to product entity
            Product product = _mapper.Map<Product>(request);
            _genericRepository.Add(product);
            if(await _genericRepository.SaveAllAsync())
            {
                ProductsResponse productResponse = _mapper.Map<ProductsResponse>(product);
                return CreatedAtAction(nameof(GetProductByIdAsync), new { id = productResponse.ProductsID }, productResponse);
            }
            return BadRequest("Problem creating product");
        }

        /// <summary>
        /// Update a product .
        /// PUT: https://localhost:portnumber/api/products/{id}
        /// </summary>
        [HttpPut]
        [Route("{id:int}")]
        [ValidateModel]
        public async Task<ActionResult<Product>> UpdateAsync([FromRoute] int id, [FromBody] AddProductRequest request)
        {
            if (id == 0 || request == null )
            {
                return BadRequest();
            }

            Product product = _mapper.Map<Product>(request);

             _genericRepository.Update(product);

            if (await _genericRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating the product");
        }


        /// <summary>
        /// Delete a product .
        /// DELETE: https://localhost:portnumber/api/products/{id}
        /// </summary>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> DeleteAsync([FromRoute] int id)
        {
            if(id == 0) return BadRequest();
            Product? product = await _genericRepository.GetByIdAsync(id);

            if (product == null) return NotFound();

            _genericRepository.Delete(product);

            if (await _genericRepository.SaveAllAsync()) 
            {
                return NoContent();
            }

            return BadRequest("Problem deleting product");
        }

        #region Validations
        private void ValidateFileUpload(ImageUploadRequest request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(request?.File?.FileName)))
            {
                ModelState.AddModelError("file", "Unsuported file extention");
            }
            if (request?.File?.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }
        #endregion
    }
}
