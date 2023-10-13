using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Data.Models
{
    public class Dish
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public decimal Rating { get; set; }

        public bool IsVegetarian { get; set; }

        public string Photo { get; set; }

        public string Category { get; set; }
    }
}
