using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie.Core.DTOs;
using Movie.Core.Services;

namespace Movie.API.Controllers
{
    [Route("api/[controller]/[action]")] // metod isminden erişim.
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // api/auth/createtoken
        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await _authenticationService.CreateTokenAsync(loginDto);
            return ActionResultInstance(result);
            /*if (result.StatusCode==200)
            {
                return Ok(result.Data);
            }
            else if(result.StatusCode==404)
            {
                return NotFound();
            }*/
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.RefreshToken);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.RefreshToken);
            return ActionResultInstance(result);
        }
    }
}
