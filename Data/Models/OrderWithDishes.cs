namespace food_delivery.Data.Models
{
    public class OrderWithDishes : Order
    {
        public List<BasketItem> Dishes { get; set; }
    }
}
