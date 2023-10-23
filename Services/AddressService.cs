using food_delivery.Data;
using food_delivery.Data.Models;
using Microsoft.EntityFrameworkCore;

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

    public List<AddressElement> GetFullAddressChain(Guid objectGuid)
    {
        var result = new List<AddressElement>();

        var initialElement = _context.AddressElements
            .SingleOrDefault(a => a.Objectguid == objectGuid && a.Isactive == 1);

        if (initialElement == null)
        {
            var initialHouse = _context.Houses
                .SingleOrDefault(a => a.Objectguid == objectGuid && a.Isactive == 1);

            if (initialHouse == null)
            {
                throw new FileNotFoundException();
            }
            else
            {
                initialElement = new AddressElement();

                initialElement.Objectguid = objectGuid;
                initialElement.Isactive = 1;
                initialElement.Id = initialHouse.Id;
                initialElement.Objectid = initialHouse.Objectid;
                initialElement.Name = initialHouse.Housenum;
                initialElement.Typename = "д";
                initialElement.Level = "10";
            }
        }

        result.Add(initialElement);

        var parentObjectGuid = initialElement.Objectguid;

        while (parentObjectGuid != Guid.Empty)
        {
            var hirerarhy = _context.Hirerarhy
                .SingleOrDefault(h => h.Objectid == initialElement.Objectid && h.Isactive == 1);

            if (hirerarhy != null)
            {
                var parentAddressElement = _context.AddressElements
                    .SingleOrDefault(a => a.Objectid == hirerarhy.Parentobjid && a.Isactive == 1);

                if (parentAddressElement != null)
                {
                    result.Add(parentAddressElement);
                    initialElement = parentAddressElement;
                    parentObjectGuid = initialElement.Objectguid;
                }
                else
                {
                    parentObjectGuid = Guid.Empty;
                }
            }
            else
            {
                parentObjectGuid = Guid.Empty;
            }
        }

        result.Reverse();

        if (!result.Any())
        {
            throw new FileNotFoundException();
        }

        return result;
    }


}
