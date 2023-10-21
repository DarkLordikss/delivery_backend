using food_delivery.Data.Models;
using food_delivery.ErrorModels;
using food_delivery.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace food_delivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly DishService _dishService;

        public DishController(DishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Get menu")]
        [Produces("application/json")]
        public ActionResult GetDishesList(
            [FromQuery]
            string[] categories = null,
            bool isVegeterian = false,
            [EnumDataType(typeof(Sorting), ErrorMessage = "Invalid sorting value.")]
            string sorting = "NameAsc",
            int page = 1,
            int pageSize = 20
            )
        {
            try
            {
                if (page < 1)
                {
                    page = 1;
                }

                var pageData = _dishService.GetDishes(page, pageSize, categories, isVegeterian, sorting);

                return Ok(pageData);
            }
            catch (FileNotFoundException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "Page not found." };

                return NotFound(errorResponce);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dish))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Get dish by ID")]
        [Produces("application/json")]
        public ActionResult GetDishById(Guid id)
        {
            try
            {
                var dish = _dishService.GetDish(id);
                return Ok(dish);
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

        [HttpGet("{id}/rating/check")]
        [Authorize(Policy = "TokenSeriesPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HasPermissionUserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Check if the user has permission to rate a dish")]
        [Produces("application/json")]
        public ActionResult CheckUserPermissionToRateDish(Guid id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                Guid.TryParse(userIdClaim.Value, out Guid parsedUserId);

                bool hasPermission = _dishService.UserHasPermissionToRateDish(id, parsedUserId);

                return Ok(new HasPermissionUserResponse { HasPermissionUserResponseToRateDish = hasPermission});
            }
            catch (FileNotFoundException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "Dish not found." };
                return NotFound(errorResponce);
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

        [HttpPost("{id}/rating")]
        [Authorize(Policy = "TokenSeriesPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IdRatingResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Rate a dish")]
        [Produces("application/json")]
        public ActionResult SetRateDish(Guid id, int ratingScore)
        {
            try
            {
                if (ratingScore < 1 || ratingScore > 5)
                {
                    var errorResponce = new ErrorResponse { ErrorMessage = "ratingScore must be between 1 and 5." };
                    return StatusCode(StatusCodes.Status400BadRequest, errorResponce);
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                Guid.TryParse(userIdClaim.Value, out Guid parsedUserId);

                int ratingId = _dishService.RateDish(id, parsedUserId, ratingScore);

                return Ok(new IdRatingResponse { RatingId = ratingId });
            }
            catch (FileNotFoundException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "Dish not found." };
                return NotFound(errorResponce);
            }
            catch (ArgumentException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "User not found." };
                return NotFound(errorResponce);
            }
            catch (MethodAccessException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "User has no permission to rate this dish." };
                return StatusCode(StatusCodes.Status403Forbidden, errorResponce);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

    }
}
