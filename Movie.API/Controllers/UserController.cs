using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie.Core.DTOs;
using Movie.Core.Services;

namespace Movie.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        public readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // Metodun tipine göre eşleşme
        // api/user
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto) // Kullanıcı oluştur.
        {            
            return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
        }

        [Authorize] // Bu end point mutlaka bir token istiyor.
        [HttpGet]
        [Route("getuser")]
        public async Task<IActionResult> GetUser()
        {
            // HttpContext.User.Claims.Where(x=x.Type == "username");
            return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }

    }
}
