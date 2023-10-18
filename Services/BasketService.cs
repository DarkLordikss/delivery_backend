using food_delivery.Data;
using food_delivery.Data.Models;
using food_delivery.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace food_delivery.Services
{
    public class BasketService
    {
        private readonly AppDbContext _context;

        public BasketService(AppDbContext context)
        {
            _context = context;
        }

        public BasketResponse GetUserBasket(Guid userId)
        {
            var dishIds = _context.DishesInCart
                .Where(item => item.UserId == userId && item.OrderId == null)
                .Select(item => item.DishId)
                .ToList();

            var userBasketItems = _context.Dishes
                .Where(dish => dishIds.Contains(dish.Id))
                .Select(dish => new BasketItem
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Amount = _context.DishesInCart
                        .Where(item => item.UserId == userId && item.DishId == dish.Id && item.OrderId == null)
                        .Sum(item => item.Count),
                    TotalPrice = dish.Price * _context.DishesInCart
                        .Where(item => item.UserId == userId && item.DishId == dish.Id && item.OrderId == null)
                        .Sum(item => item.Count),
                    Photo = dish.Photo
                })
                .ToList();

            decimal total = userBasketItems.Sum(item => item.Price * item.Amount);

            var userBasket = new BasketResponse
            {
                Items = userBasketItems,
                Total = new BasketTotal
                {
                    ItemsCount = userBasketItems.Count,
                    TotalPrice = total
                }
            };

            return userBasket;
        }

        public int? AddDishToBasket(Guid userId, Guid dishId, int count)
        {
            var dish = _context.Dishes.SingleOrDefault(d => d.Id == dishId);
            if (dish == null)
            {
                return null;
            }

            var existingCartItem = _context.DishesInCart
                .SingleOrDefault(item => item.UserId == userId && item.DishId == dishId && item.OrderId == null);

            if (existingCartItem != null)
            {
                existingCartItem.Count += count;
            }
            else
            {
                var newCartItem = new DishInCart
                {
                    UserId = userId,
                    DishId = dishId,
                    Count = count
                };

                _context.DishesInCart.Add(newCartItem);
            }

            _context.SaveChanges();

            existingCartItem = _context.DishesInCart
                .SingleOrDefault(item => item.UserId == userId && item.DishId == dishId && item.OrderId == null);

            return existingCartItem?.Id;
        }

        public int? DecreaseOrRemoveDishFromBasket(Guid userId, Guid dishId, bool increase)
        {
            var cartItem = _context.DishesInCart
                .SingleOrDefault(item => item.UserId == userId && item.DishId == dishId && item.OrderId == null);

            if (cartItem == null)
            {
                return null;
            }

            if (increase)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count -= 1;
                }
                else
                {
                    _context.DishesInCart.Remove(cartItem);
                }
            }
            else
            {
                _context.DishesInCart.Remove(cartItem);
            }

            _context.SaveChanges();

            return cartItem.Id;
        }


    }

}
