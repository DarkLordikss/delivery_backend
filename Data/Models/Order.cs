using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace food_delivery.Data.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime OrderTime { get; set; }

        public DateTime DeliveryTime { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("AddressId")]
        public int AddressId { get; set; }

        public Status Status { get; set; }
    }
}
