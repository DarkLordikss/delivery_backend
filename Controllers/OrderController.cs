﻿using food_delivery.Data.Models;
using food_delivery.ErrorModels;
using food_delivery.RequestModels;
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
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
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

        [HttpGet]
        [Authorize(Policy = "TokenSeriesPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IQueryable<Order>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Get a list of orders")]
        [Produces("application/json")]
        public IActionResult GetOrdersList()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                Guid.TryParse(userIdClaim.Value, out Guid parsedUserId);

                var orders = _orderService.GetOrders(parsedUserId);

                return Ok(orders);
            }
            catch (ArgumentException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "User not found." };
                return NotFound(errorResponce);
            }
            catch (FileNotFoundException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "Orders not found." };
                return NotFound(errorResponce);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [Authorize(Policy = "TokenSeriesPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GuidOrderResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Create order from cart")]
        [Produces("application/json")]
        public IActionResult CreateOrderFromCart(OrderCreateRequest orderData)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                Guid.TryParse(userIdClaim.Value, out Guid parsedUserId);

                var orderId = _orderService.CreateOrder(parsedUserId, orderData);

                return Ok(new GuidOrderResponse { OrderId = orderId });
            }
            catch (ArgumentException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "User not found." };
                return NotFound(errorResponce);
            }
            catch (FileNotFoundException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "Dishes in cart not found." };
                return NotFound(errorResponce);
            }
            catch (KeyNotFoundException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "Address not exist." };
                return NotFound(errorResponce);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpPost("{id}/status")]
        [Authorize(Policy = "TokenSeriesPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GuidOrderResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Confirm order delivery")]
        [Produces("application/json")]
        public IActionResult ConfirmOrder(Guid id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                Guid.TryParse(userIdClaim.Value, out Guid parsedUserId);

                var orderId = _orderService.ConfirmOrder(id, parsedUserId);

                return Ok(new GuidOrderResponse { OrderId = orderId });
            }
            catch (DuplicateWaitObjectException ex)
            {
                var errorResponce = new ErrorResponse { ErrorMessage = "Order already has status 'Delivered'." };
                return StatusCode(StatusCodes.Status403Forbidden, errorResponce);
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
