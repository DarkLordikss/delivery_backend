using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace food_delivery.Data.Models
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Value { get; set; }

        [ForeignKey("DishId")]
        public Guid DishId { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
    }
}
