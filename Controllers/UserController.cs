using food_delivery.Data.Models;
using food_delivery.Errors;
using food_delivery.Responses;
using food_delivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace food_delivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TokenService _tokenService;

        public UserController(UserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public ActionResult Register(UserRegistrationModel user)
        {
            try
            {
                var userId = _userService.RegisterUser(user);

                var token = _tokenService.GenerateToken(userId);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
