using System.ComponentModel.DataAnnotations;

namespace food_delivery.RequestModels
{
    public class OrderCreateRequest
    {
        [DataType(DataType.DateTime, ErrorMessage = "Birth date must be a valid date.")]
        public DateTime DeliveryTime { get; set; }

        public Guid AddressId { get; set; }
    }
}
