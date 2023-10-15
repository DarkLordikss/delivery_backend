using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace food_delivery.Data.Models
{
    public class User
    {
        [Key]
        [IgnoreDataMember]
        [DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        [ForeignKey("AddressId")]
        public Guid Addressid { get; set; }
    }
}
