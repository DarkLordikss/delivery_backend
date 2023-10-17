using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using food_delivery.Data;
using food_delivery.Data.Models;

public class TokenService
{
    private readonly AppDbContext _context;

    public TokenService(AppDbContext context)
    {
        _context = context;
    }

    public long? GetTokenSeriesByUserId(Guid userId)
    {
        var userPassword = _context.Passwords
            .FirstOrDefault(p => p.UserId == userId);

        return userPassword?.TokenSeries;
    }
}
