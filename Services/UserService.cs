using System;
using System.Security.Cryptography;
using System.Text;
using food_delivery.Data;
using food_delivery.Data.Models;

namespace food_delivery.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public Guid RegisterUser(UserRegistrationModel user)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(user.Password, salt);
            Guid userId = Guid.NewGuid();

            var newUser = new User
            {
                Id = userId,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Phone = user.Phone,
                Email = user.Email,
                Addressid = user.Addressid,
            };

            var newPassword = new Password
            {
                UserId = userId,
                Salt = salt,
                PasswordHash = hashedPassword
            };

            _context.Passwords.Add(newPassword);
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return userId;
        }

        private string GenerateSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        private string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPassword = new byte[saltBytes.Length + passwordBytes.Length];

            Array.Copy(saltBytes, 0, saltedPassword, 0, saltBytes.Length);
            Array.Copy(passwordBytes, 0, saltedPassword, saltBytes.Length, passwordBytes.Length);

            using (var sha256 = SHA256.Create())
            {
                byte[] hashedPassword = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hashedPassword);
            }
        }

    }
}
