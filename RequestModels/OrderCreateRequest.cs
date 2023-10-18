namespace food_delivery.RequestModels
{
    public class OrderCreateRequest
    {
        public DateTime DeliveryTime { get; set; }

        public Guid AddressId { get; set; }
    }
}
