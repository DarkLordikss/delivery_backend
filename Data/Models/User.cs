using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Data.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        [ForeignKey("AddressId")]
        public Guid AddressId { get; set; }
    }
}
