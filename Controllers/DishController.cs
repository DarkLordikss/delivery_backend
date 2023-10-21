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
        private readonly TokenService _tokenService;

        public DishController(DishService dishService, TokenService tokenService)
        {
            _dishService = dishService;
            _tokenService = tokenService;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
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


    }
}
