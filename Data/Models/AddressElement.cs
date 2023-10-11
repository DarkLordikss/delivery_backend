using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Data.Models
{
    public class AddressElement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Objectid { get; set; }

        public Guid Objectguid { get; set; }

        public string Name { get; set; }

        public string Typename { get; set; }

        public string Level { get; set; }

        public bool Isactive { get; set; }
    }
}
