using System.ComponentModel.DataAnnotations;

namespace food_delivery.RequestModels
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
