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
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{orderId}")]
        [Authorize(Policy = "TokenSeriesPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderWithDishesResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Get info about order")]
        [Produces("application/json")]
        public IActionResult GetOrder(Guid orderId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                Guid.TryParse(userIdClaim.Value, out Guid parsedUserId);

                var order = _orderService.GetOrderInfo(orderId, parsedUserId);

                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "User not found." };
                return NotFound(errorResponce);
            }
            catch (FileNotFoundException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "Order not found." };
                return NotFound(errorResponce);
            }
            catch (MethodAccessException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "User has no access for this order." };
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
