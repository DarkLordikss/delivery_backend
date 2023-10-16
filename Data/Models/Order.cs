using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace food_delivery.Data.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime OrderTime { get; set; }

        public DateTime? DeliveryTime { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("AddressId")]
        public Guid AddressId { get; set; }

        public string Status { get; set; }
    }
}
