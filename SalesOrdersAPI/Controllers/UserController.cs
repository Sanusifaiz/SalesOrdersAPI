using Microsoft.AspNetCore.Mvc;
using SalesOrders.Domains.DTOs;
using SalesOrders.Domains.Models;
using SalesOrders.Services.IServices;
using SalesOrdersAPI.Validations;

namespace SalesOrdersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("signup")]
        public async Task<IActionResult> Signup(UserRegRequestDTO userRegisterDto)
        {
            //validate request
            var validator = new UserRegRequestValidator().Validate(userRegisterDto);
            if (!validator.IsValid)
            {
                return BadRequest(ResponseMessage<string>.Fail(validator.Errors.First().ErrorMessage));
            }
            var result = await _authService.Register(userRegisterDto.UserName, userRegisterDto.Password);

            if (result.Error)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDto)
        {

            //validate request
            var validator = new UserRegRequestValidator().Validate(userLoginDto);
            if (!validator.IsValid)
            {
                return BadRequest(ResponseMessage<string>.Fail(validator.Errors.First().ErrorMessage));
            }

            var result = await _authService.Login(userLoginDto.UserName, userLoginDto.Password);

            if (result.Error)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
