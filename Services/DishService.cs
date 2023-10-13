using food_delivery.Data.Models;
using food_delivery.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

public class DishService
{
    private readonly AppDbContext _context;

    public DishService(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Dish> GetDishes(
        int page = 1,
        int pageSize = 20,
        string[] categories = null,
        bool vegetarian = false,
        string sorting = "NameAsc"
    )
    {
        var query = _context.Dishes.AsQueryable();

        if (categories != null && categories.Any())
        {
            query = query.Where(d => categories.Contains(d.Category));
        }

        if (vegetarian)
        {
            query = query.Where(d => d.IsVegetarian);
        }

        switch (sorting)
        {
            case "NameAsc":
                query = query.OrderBy(d => d.Name);
                break;
            case "NameDesc":
                query = query.OrderByDescending(d => d.Name);
                break;
            case "PriceAsc":
                query = query.OrderBy(d => d.Price);
                break;
            case "PriceDesc":
                query = query.OrderByDescending(d => d.Price);
                break;
            case "RatingAsc":
                query = query.OrderBy(d => d.Rating);
                break;
            case "RatingDesc":
                query = query.OrderByDescending(d => d.Rating);
                break;
            default:
                break;
        }

        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public Dish GetDish(Guid id)
    {
        return _context.Dishes.SingleOrDefault(d => d.Id == id);
    }

    public bool UserHasPermissionToRateDish(Guid dishId, Guid userId)
    {
        var cartItems = _context.DishesInCart
            .Where(item => item.DishId == dishId && item.UserId == userId)
            .ToList();

        return cartItems.Any(cartItem =>
        {
            var order = _context.Orders.SingleOrDefault(o => o.Id == cartItem.OrderId);
            return order != null && order.Status == "Delivered";
        });
    }

    public bool RateDish(Guid dishId, Guid userId, int ratingValue)
    {
        if (!UserHasPermissionToRateDish(dishId, userId))
        {
            return false;
        }

        var existingRating = _context.Ratings
            .SingleOrDefault(r => r.DishId == dishId && r.UserId == userId);

        if (existingRating != null)
        {
            existingRating.Value = ratingValue;
        }
        else
        {
            var newRating = new Rating
            {
                DishId = dishId,
                UserId = userId,
                Value = ratingValue
            };

            _context.Ratings.Add(newRating);
        }

        _context.SaveChanges();

        UpdateDishRating(dishId);

        return true;
    }


    private void UpdateDishRating(Guid dishId)
    {
        var ratings = _context.Ratings.Where(r => r.DishId == dishId).ToList();

        if (ratings.Any())
        {
            decimal averageRating = (decimal)ratings.Average(r => r.Value);

            var dish = _context.Dishes.SingleOrDefault(d => d.Id == dishId);
            if (dish != null)
            {
                dish.Rating = averageRating;
                _context.SaveChanges();
            }
        }
    }

}
