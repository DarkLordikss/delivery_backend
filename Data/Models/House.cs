using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Data.Models
{
    public class House
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long Objectid { get; set; }

        public Guid Objectguid { get; set; }

        public string Housenum { get; set; }

        public string? Addnum1 { get; set; }

        public string? Addnum2 { get; set; }

        public int? Addtype1 { get; set; }

        public int? Addtype2 { get; set; }

        public int Isactive { get; set; }
    }
}
