using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        //Post: /api/AuthRegister

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto  registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded) 
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any()) 
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded) 
                    {
                        return Ok("User was registered. Please login.");
                    }
                }

            }
            return BadRequest("Something went wrong");
        }


        //Post: /api/Auth/Login

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
           var user= await _userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
               var checkPasswordResult= await _userManager.CheckPasswordAsync(user,loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    //Get roles

                   var roles= await _userManager.GetRolesAsync(user);

                    if (roles != null) 
                    {
                        // Create Token

                        var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                    

                    return Ok();
                }
            }
            return BadRequest("Username or password incorrect");
        }
    }
}
