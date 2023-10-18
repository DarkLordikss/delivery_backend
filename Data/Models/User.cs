using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

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

        public Guid Addressid { get; set; }
    }
}
