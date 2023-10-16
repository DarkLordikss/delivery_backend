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

        private IQueryable<BasketItem> GetOrderItems(Guid orderId)
        {
            var dishIds = _context.DishesInCart
                .Where(item => item.OrderId == orderId)
                .Select(item => item.DishId);

            var basketItems = _context.Dishes
                .Where(dish => dishIds.Contains(dish.Id))
                .Select(dish => new BasketItem
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Amount = _context.DishesInCart
                        .Where(item => item.DishId == dish.Id && item.OrderId == orderId)
                        .Sum(item => item.Count),
                    TotalPrice = dish.Price * _context.DishesInCart
                        .Where(item => item.DishId == dish.Id && item.OrderId == orderId)
                        .Sum(item => item.Count),
                    Photo = dish.Photo
                });

            return basketItems;
        }
    }
}
