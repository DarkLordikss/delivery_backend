namespace food_delivery.Data.Models
{
    public class CreateOrderModel
    {
        public DateTime DeliveryTime { get; set; }

        public Guid AddressId { get; set; }
    }
}
