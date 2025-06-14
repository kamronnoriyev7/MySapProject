using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySapProject.Application.Services;
using MySapProject.Domain.Entities;

namespace MySapProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ILogoutService _logoutService;

        public AuthController(ILoginService loginService, ILogoutService logoutService)
        {
            _loginService = loginService;
            _logoutService = logoutService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Login request cannot be null.");
            }

            return Ok(await _loginService.LoginAsync(loginRequest));
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _logoutService.LogoutAndClearSessionAsync();
           
            return Ok(result);
        }
    }
}
