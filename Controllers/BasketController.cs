using food_delivery.ErrorModels;
using food_delivery.ResponseModels;
using food_delivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace food_delivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly BasketService _basketService;

        public BasketController(BasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        [Authorize(Policy = "TokenSeriesPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Get user cart")]
        [Produces("application/json")]
        public IActionResult GetBasket()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                Guid.TryParse(userIdClaim.Value, out Guid parsedUserId);

                var cart = _basketService.GetUserBasket(parsedUserId);

                return Ok(cart);
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

        [HttpPost("dish/{dishId}")]
        [Authorize(Policy = "TokenSeriesPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IdDishInCartResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Add dish to cart")]
        [Produces("application/json")]
        public IActionResult AddDish(Guid dishId, int count = 1)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                Guid.TryParse(userIdClaim.Value, out Guid parsedUserId);

                int dishInCartId = _basketService.AddDishToBasket(parsedUserId, dishId, count);

                return Ok(new IdDishInCartResponse { DishInCartId = dishInCartId });
            }
            catch (ArgumentException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "User not found." };
                return NotFound(errorResponce);
            }
            catch (FileNotFoundException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "Dish not found." };
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
