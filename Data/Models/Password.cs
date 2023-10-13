using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace food_delivery.Data.Models
{
    public class Password
    {
        [Key]
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public string PasswordHash { get; set; }
    }
}
