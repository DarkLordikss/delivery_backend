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
        List<string> categories = null,
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

}
