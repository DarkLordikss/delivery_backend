using food_delivery.Data.Models;
using food_delivery.Data;
using Microsoft.EntityFrameworkCore;
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
        List<string> categories = null,
        bool vegetarian = false,
        string sorting = "NameAsc"
    )
    {
        var dishes = _context.Dishes;
        var ratings = _context.Ratings;

        var query = _context.Dishes
            .Join(_context.Ratings, d => d.Id, r => r.DishId, (dish, rating) => new { Dish = dish, Rating = rating });


        if (categories != null && categories.Any())
        {
            query = query.Where(d => categories.Contains(d.Dish.Category));
        }

        if (vegetarian == true)
        {
            query = query.Where(d => d.Dish.IsVegetarian == vegetarian);
        }

        switch (sorting)
        {
            case "NameAsc":
                query = query.OrderBy(d => d.Dish.Name);
                break;
            case "NameDesc":
                query = query.OrderByDescending(d => d.Dish.Name);
                break;
            case "PriceAsc":
                query = query.OrderBy(d => d.Dish.Price);
                break;
            case "PriceDesc":
                query = query.OrderByDescending(d => d.Dish.Price);
                break;
            case "RatingAsc":
                query = query.OrderBy(d => d.Rating.Value);
                break;
            case "RatingDesc":
                query = query.OrderByDescending(d => d.Rating.Value);
                break;
            default:
                break;
        }


        return query.Select(d => d.Dish).Skip((page - 1) * pageSize).Take(pageSize);
    }
}
