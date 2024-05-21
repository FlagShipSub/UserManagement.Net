using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagment.Models;
using UserManagment.Models.Dtos;
using UserManagment.Service.IService;

namespace UserManagment.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly    IAuthService _authService;
        protected Response<object>? _response;


        public AuthController(IAuthService authService,IJwtTokenGenerator jwtTokenGenerator)
        {
               _authService = authService;
            
        }

        [HttpPost("register")] 
        public async Task<IActionResult> RegisterUser(RegisterationRequestDto registerationRequestDto)
        {
            var response = await _authService.Register(registerationRequestDto);

            return Ok(response);

        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var response = await _authService.Login(loginRequestDto);
            return Ok(response);

        }
        [HttpPost("generateotp/email")]

        public async Task<IActionResult> ResetPassword(VerificationRequest verificationRequest)
        {
            var response = await _authService.GenerateOtp(verificationRequest);
            return Ok(response);

        }
        [HttpGet("Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _authService.GetAllUsers();
            return Ok(response);
        }
    }
}
