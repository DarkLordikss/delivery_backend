using food_delivery.Data.Models;

namespace food_delivery.ResponseModels
{
    public class OrderWithDishesResponse : Order
    {
        public List<BasketItem> Dishes { get; set; }
    }
}
