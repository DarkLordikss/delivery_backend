using food_delivery.Data;
using food_delivery.Data.Models;

namespace food_delivery.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        private List<BasketItem> GetOrderItems(Guid userId, Guid orderId)
        {
            var dishIds = _context.DishesInCart
                .Where(item => item.UserId == userId && item.OrderId == orderId)
                .Select(item => item.DishId)
                .ToList();

            var basketItems = _context.Dishes
                .Where(dish => dishIds.Contains(dish.Id))
                .Select(dish => new BasketItem
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Amount = _context.DishesInCart
                        .Where(item => item.UserId == userId && item.DishId == dish.Id && item.OrderId == orderId)
                        .Sum(item => item.Count),
                    TotalPrice = dish.Price * _context.DishesInCart
                        .Where(item => item.UserId == userId && item.DishId == dish.Id && item.OrderId == orderId)
                        .Sum(item => item.Count),
                    Photo = dish.Photo
                })
                .ToList();

            return basketItems;
        }
    }
}
