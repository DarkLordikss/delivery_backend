﻿using System;
using System.Security.Cryptography;
using System.Text;
using food_delivery.Data;
using food_delivery.Data.Models;
using food_delivery.RequestModels;

namespace food_delivery.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public Guid RegisterUser(UserRegistrationRequest user)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(user.Password, salt);
            Guid userId = Guid.NewGuid();

            var existingUser = _context.Users.SingleOrDefault(u => u.Email == user.Email || u.Phone == user.Phone);

            var existingAddress = _context.Houses.SingleOrDefault(a => a.Objectguid == user.Addressid && a.Isactive == 1);

            if (existingUser != null)
            {
                throw new ArgumentException();
            }

            if (existingAddress == null)
            {
                throw new FileNotFoundException();
            }

            var newUser = new User
            {
                Id = userId,
                FullName = user.FullName,
                BirthDate = user.BirthDate.ToUniversalTime(),
                Gender = user.Gender,
                Phone = user.Phone,
                Email = user.Email,
                Addressid = user.Addressid,
            };

            var newPassword = new Password
            {
                UserId = userId,
                Salt = salt,
                PasswordHash = hashedPassword,
                TokenSeries = 0
            };

            _context.Passwords.Add(newPassword);
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return userId;
        }

        public Guid LogoutUser(Guid userId)
        {
            var passwordData = _context.Passwords.SingleOrDefault(u => u.UserId == userId);

            if (passwordData == null)
            {
                throw new ArgumentException();
            }

            passwordData.TokenSeries += 1;

            _context.Passwords.Update(passwordData);
            _context.SaveChanges();

            return userId;
        }

        public Guid LoginUser(UserLoginRequest loginData)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == loginData.Email);

            if (user == null)
            {
                throw new ArgumentException();
            }

            var passwordRecord = _context.Passwords.SingleOrDefault(u => u.UserId == user.Id);

            if (passwordRecord == null)
            {
                throw new ArgumentException();
            }

            string hashedPassword = HashPassword(loginData.Password, passwordRecord.Salt);

            if (hashedPassword == passwordRecord.PasswordHash)
            {
                return user.Id;
            }

            throw new ArgumentException();
        }

        public User GetUser(Guid userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentException();
            }

            return user;
        }

        public Guid EditUser(UserEditRequest newUserData, Guid userId)
        {
            var oldUser = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (oldUser == null)
            {
                throw new ArgumentException();
            }

            var existingAddress = _context.Houses.SingleOrDefault(a => a.Objectguid == newUserData.Addressid && a.Isactive == 1);
            var existingNumber = _context.Users.SingleOrDefault(u => u.Phone == newUserData.Phone && u.Id != userId);

            if (existingNumber != null)
            {
                throw new DuplicateWaitObjectException();
            }

            if (existingAddress == null)
            {
                throw new FileNotFoundException();
            }

            oldUser.FullName = newUserData.FullName;
            oldUser.Addressid = newUserData.Addressid;
            oldUser.BirthDate = newUserData.BirthDate;
            oldUser.Gender = newUserData.Gender;
            oldUser.Phone = newUserData.Phone;

            _context.Users.Update(oldUser);
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
