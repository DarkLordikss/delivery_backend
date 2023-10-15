using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Data.Models
{
    public class UserEditModel
    {
        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public Guid Addressid { get; set; }
    }
}
