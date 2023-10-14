using food_delivery.Data;
using food_delivery.Data.Models;

namespace food_delivery.Services
{
    public class BasketService
    {
        private readonly AppDbContext _context;

        public BasketService(AppDbContext context)
        {
            _context = context;
        }

        public Basket GetUserBasket(Guid userId)
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

            var userBasket = new Basket
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
    }
}
