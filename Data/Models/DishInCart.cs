using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Data.Models
{
    public class DishInCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("DishId")]
        public Guid DishId { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        [ForeignKey("OrderId")]
        public Guid? OrderId { get; set; }

        public int Count { get; set; }
    }
}
