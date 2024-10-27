using AutoMapper;
using Core.Models.Domain;
using Core.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JediOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountController(SignInManager<AppUser> signInManager, IMapper mapper, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="registerRequest">The user details for the account creation.</param>
        [HttpPost, AllowAnonymous]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            AppUser user = _mapper.Map<AppUser>(registerRequest);

            IdentityResult? result = await _signInManager.UserManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded) return BadRequest();

            return Ok(); 
        }
    }

}
