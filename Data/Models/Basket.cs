namespace food_delivery.Data.Models
{
    public class Basket
    {
        public List<BasketItem> Items { get; set; }
        public BasketTotal Total { get; set; }

        public Basket()
        {
            Items = new List<BasketItem>();
            Total = new BasketTotal();
        }
    }
}
