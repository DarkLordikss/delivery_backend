using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Data.Models
{
    public class Hirerarhy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Objectid")]
        public long Objectid { get; set; }

        [ForeignKey("Parentid")]
        public long Parentobjid { get; set; }

        public int Isactive { get; set; }
    }
}
