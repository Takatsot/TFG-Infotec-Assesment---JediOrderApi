using Core.Models.Domain;
using Core.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace JediOrderApi.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("Not a good request");
        }
        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }
        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            throw new Exception("Oops something wrong happed");
        }
        [HttpPost("validationerror")]
        public IActionResult GetValidationError(AddProductRequest products)
        {
            return Ok();
        }     
    }
}
