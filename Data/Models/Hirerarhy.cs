using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Data.Models
{
    public class Hirerarhy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Objectid")]
        public int Objectid { get; set; }

        [ForeignKey("Parentid")]
        public int Parentobjid { get; set; }

        public bool Isactive { get; set; }
    }
}
