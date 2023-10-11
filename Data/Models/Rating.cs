using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace food_delivery.Data.Models
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Value { get; set; }

        [ForeignKey("DishId")]
        public int DishId { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
    }
}
