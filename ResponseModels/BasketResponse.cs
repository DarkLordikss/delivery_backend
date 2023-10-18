using food_delivery.Data.Models;

namespace food_delivery.ResponseModels
{
    public class BasketResponse
    {
        public List<BasketItem> Items { get; set; }
        public BasketTotal Total { get; set; }

        public BasketResponse()
        {
            Items = new List<BasketItem>();
            Total = new BasketTotal();
        }
    }
}
