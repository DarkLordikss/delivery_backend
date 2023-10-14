using food_delivery.Data;
using food_delivery.Data.Models;

public class AddressService
{
    private readonly AppDbContext _context;

    public AddressService(AppDbContext context)
    {
        _context = context;
    }

    public List<AddressElement> SearchAddressElements(string query, long parentObjectId)
    {
        query = query.ToLower();

        var childObjectIds = _context.Hirerarhy
            .Where(h => h.Parentobjid == parentObjectId)
            .Select(h => h.Objectid)
            .ToList();

        var addressElements = _context.AddressElements
            .Where(a => a.Name.ToLower().Contains(query) && childObjectIds.Contains(a.Objectid))
            .ToList();

        return addressElements;
    }
}
