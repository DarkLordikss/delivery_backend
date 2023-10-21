using food_delivery.Data.Models;

namespace food_delivery.ResponseModels
{
    public class MenuResponse
    {
        public List<Dish> Dishes { get; set; }
        public Pagination Pagination { get; set; }
    }
}
