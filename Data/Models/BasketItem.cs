namespace food_delivery.Data.Models
{
    public class BasketItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Photo { get; set; }
    }

}
