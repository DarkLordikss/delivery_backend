using food_delivery.Data.Models;
using food_delivery.Errors;
using food_delivery.Responses;
using food_delivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Register new user")]
        [Produces("application/json")]
        public ActionResult Register(UserRegistrationModel user)
        {
            try
            {
                var userId = _userService.RegisterUser(user);

                var token = _tokenService.GenerateToken(userId);

                return Ok(new TokenResponse { Token = token });
            }
            catch (ArgumentException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "This user already exist." };

                return Conflict(errorResponce);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Login into the system")]
        [Produces("application/json")]
        public ActionResult Login(LoginModel user)
        {
            try
            {
                var userId = _userService.LoginUser(user);

                var token = _tokenService.GenerateToken(userId);

                return Ok(new TokenResponse { Token = token });
            }
            catch (ArgumentException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "Invalid email or password." };

                return Conflict(errorResponce);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPost("logout")]
        [Authorize(Policy = "TokenSeriesPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GuidUserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Logout from the system")]
        [Produces("application/json")]
        public ActionResult Logout()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                Guid.TryParse(userIdClaim.Value, out Guid parsedUserId);

                var userId = _userService.LogoutUser(parsedUserId);

                return Ok(new GuidUserResponse { UserId = userId });

            }
            catch (ArgumentException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "User not found." };

                return NotFound(errorResponce);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet("profile")]
        [Authorize(Policy = "TokenSeriesPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Get user profile")]
        [Produces("application/json")]
        public ActionResult GetUserProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                Guid.TryParse(userIdClaim.Value, out Guid parsedUserId);

                var user = _userService.GetUser(parsedUserId);

                return Ok(user);

            }
            catch (ArgumentException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "User not found." };

                return NotFound(errorResponce);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPut("profile")]
        [Authorize(Policy = "TokenSeriesPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GuidUserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Edit user profile")]
        [Produces("application/json")]
        public ActionResult EditUserProfile(UserEditModel newUserData)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                Guid.TryParse(userIdClaim.Value, out Guid parsedUserId);

                var userId = _userService.EditUser(newUserData, parsedUserId);

                return Ok(new GuidUserResponse { UserId = userId });

            }
            catch (ArgumentException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "User not found." };

                return NotFound(errorResponce);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

    }
}
