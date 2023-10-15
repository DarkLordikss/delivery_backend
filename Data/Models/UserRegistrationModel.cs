namespace food_delivery.Data.Models
{
    public class UserRegistrationModel : User
    {
        public string PasswordHash { get; set; }
    }
}
