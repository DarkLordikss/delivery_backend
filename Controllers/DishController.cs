using food_delivery.Data.Models;
using food_delivery.ErrorModels;
using food_delivery.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

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
    }
}
