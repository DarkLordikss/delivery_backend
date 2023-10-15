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
    }
}
