using System.ComponentModel.DataAnnotations;

namespace food_delivery.RequestModels
{
    public class OrderCreateRequest
    {
        [Required(ErrorMessage = "DeliveryTime is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Birth date must be a valid date.")]
        public DateTime DeliveryTime { get; set; }

        [Required(ErrorMessage = "AddressId is required.")]
        public Guid AddressId { get; set; }
    }
}
