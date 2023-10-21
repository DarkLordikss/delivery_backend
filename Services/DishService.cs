using food_delivery.Data.Models;
using food_delivery.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using food_delivery.ResponseModels;

public class DishService
{
    private readonly AppDbContext _context;

    public DishService(AppDbContext context)
    {
        _context = context;
    }

    public MenuResponse GetDishes(
        int page,
        int pageSize,
        string[] categories,
        bool vegetarian,
        string sorting
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

        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        if (page > totalPages)
        {
            throw new FileNotFoundException();
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

        var dishes = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var pagination = new Pagination
        {
            Size = pageSize,
            Count = totalPages,
            Current = page
        };

        var menuResponse = new MenuResponse
        {
            Dishes = dishes,
            Pagination = pagination
        };

        return menuResponse;
    }


    public Dish GetDish(Guid id)
    {
        var dish = _context.Dishes.SingleOrDefault(d => d.Id == id);

        if (dish == null)
        {
            throw new FileNotFoundException();
        }

        return dish;
    }

    public bool UserHasPermissionToRateDish(Guid dishId, Guid userId)
    {
        GetDish(dishId);

        var cartItems = _context.DishesInCart
            .Where(item => item.DishId == dishId && item.UserId == userId)
            .ToList();

        return cartItems.Any(cartItem =>
        {
            var order = _context.Orders.SingleOrDefault(o => o.Id == cartItem.OrderId);
            return order != null && order.Status == "Delivered";
        });
    }

    public int RateDish(Guid dishId, Guid userId, int ratingValue)
    {
        if (!UserHasPermissionToRateDish(dishId, userId))
        {
            throw new MethodAccessException();
        }

        var existingRating = _context.Ratings
            .SingleOrDefault(r => r.DishId == dishId && r.UserId == userId);

        if (existingRating != null)
        {
            existingRating.Value = ratingValue;

            _context.SaveChanges();

            return existingRating.Id;
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
            _context.SaveChanges();

            return newRating.Id;
        }
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
