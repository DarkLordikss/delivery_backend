using food_delivery.Data.Models;
using food_delivery.ErrorModels;
using food_delivery.ResponseModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace food_delivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;

        public AddressController(AddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AddressElement>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Searching child address elements")]
        [Produces("application/json")]
        public ActionResult Search(long parentObjectId, string query)
        {
            try
            {
                var addressElements = _addressService.SearchAddressElements(query, parentObjectId);

                return Ok(addressElements);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet("getaddresschain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AddressElement>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(Summary = "Give full address chain for element")]
        [Produces("application/json")]
        public ActionResult GetAddressChain(Guid objectGuid)
        {
            try
            {
                var addressChain = _addressService.GetFullAddressChain(objectGuid);

                return Ok(addressChain);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse { ErrorMessage = "An internal server error occurred." };

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

    }
}
