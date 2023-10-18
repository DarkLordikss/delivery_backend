using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Data.Models
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 30 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Birth date must be a valid date.")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender value. Can be only 'Male' or 'Female'")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(@"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$", ErrorMessage = "Phone must match the format +7 (xxx) xxx-xx-xx.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [ForeignKey("AddressId")]
        public Guid Addressid { get; set; }
    }
}
