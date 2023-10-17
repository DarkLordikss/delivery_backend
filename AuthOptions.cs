using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace food_delivery
{
    public class AuthOptions
    {
        public const string ISSUER = "BlanckProject";
        public const string AUDIENCE = "DeliveryFrontend";
        const string KEY = "fm2jf0923u1jJ!)((!@(@J193ej;askd-0oduwjqljasdsn";
        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
