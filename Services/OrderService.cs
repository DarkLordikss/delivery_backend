using food_delivery.Data;

namespace food_delivery.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
    }
}
