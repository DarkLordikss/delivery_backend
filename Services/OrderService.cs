using food_delivery.Data;
using food_delivery.Data.Models;
using food_delivery.RequestModels;
using food_delivery.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace food_delivery.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }
        public OrderWithDishesResponse GetOrderInfo(Guid orderId, Guid userId)
        {
            var order = _context.Orders.SingleOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                throw new FileNotFoundException();
            }

            var user = _context.DishesInCart.FirstOrDefault(d => d.UserId == userId && d.OrderId == orderId);

            if (user == null)
            {
                throw new MethodAccessException();
            }

            var dishes = GetOrderItems(order.Id);

            var orderWithDishes = new OrderWithDishesResponse
            {
                Id = orderId,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Status = order.Status,
                Price = dishes.Sum(item => item.TotalPrice),
                Dishes = dishes.ToList(),
                AddressId = order.AddressId,
            };

            return orderWithDishes;
        }

        public IQueryable<Order> GetOrders(Guid userId)
        {
            var uniqueOrderIds = _context.DishesInCart
                .Where(d => d.UserId == userId && d.OrderId != null)
                .Select(d => d.OrderId)
                .Distinct()
                .ToList();

            var orders = _context.Orders
                .Where(o => uniqueOrderIds.Contains(o.Id))
                .AsQueryable();

            if (!orders.Any())
            {
                throw new FileNotFoundException();
            }

            return orders;
        }

        public Guid CreateOrder(Guid userId, OrderCreateRequest createOrderModel)
        {
            var newOrderId = Guid.NewGuid();

            var cartItems = _context.DishesInCart
                .Where(d => d.UserId == userId && d.OrderId == null)
                .ToList();

            if (cartItems == null)
            {
                throw new FileNotFoundException();
            }

            var existingAddress = _context.Houses.SingleOrDefault(a => a.Objectguid == createOrderModel.AddressId && a.Isactive == 1);

            if (existingAddress != null)
            {
                throw new KeyNotFoundException();
            }

            foreach (var cartItem in cartItems)
            {
                cartItem.OrderId = newOrderId;
            }

            _context.SaveChanges();

            decimal totalOrderPrice = cartItems.Sum(item => _context.Dishes.Single(d => d.Id == item.DishId).Price * item.Count);

            var newOrder = new Order
            {
                Id = newOrderId,
                OrderTime = DateTime.Now.ToUniversalTime(),
                DeliveryTime = createOrderModel.DeliveryTime,
                AddressId = createOrderModel.AddressId,
                Status = "inProcess",
                Price = totalOrderPrice,
            };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            return newOrderId;
        }

        public Guid ConfirmOrder(Guid orderId, Guid userId)
        {
            var order = _context.Orders.SingleOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                throw new FileNotFoundException();
            }

            var user = _context.DishesInCart.FirstOrDefault(d => d.UserId == userId && d.OrderId == orderId);

            if (user == null)
            {
                throw new MethodAccessException();
            }

            order.Status = "Delivered";
            _context.SaveChanges();

            return orderId;
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
