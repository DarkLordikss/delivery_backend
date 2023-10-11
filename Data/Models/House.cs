using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Data.Models
{
    public class House
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Objectid { get; set; }

        public Guid Objectguid { get; set; }

        public string Housenum { get; set; }

        public string Addnum1 { get; set; }

        public string Addnum2 { get; set; }

        public string Addtype1 { get; set; }

        public string Addtype2 { get; set; }

        public bool Isactive { get; set; }
    }
}
