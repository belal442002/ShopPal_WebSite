using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> GetUserOrders()
        {
            var userId = GetUserId();
            if (userId is null)
                throw new Exception("User not logged-in");
            var orders = await _dbContext.Orders
                                   .Include(o => o.OrderStatus)
                                   .Include(o => o.OrderDetails)!
                                   .ThenInclude(o => o.Product)
                                   .ThenInclude(p => p!.Category)
                                   .Where(o => o.UserId == userId)
                                   .ToArrayAsync();
            return orders;
        }

        private String GetUserId()
        {
            var user = _httpContextAccessor.HttpContext!.User;
            var userId = _userManager.GetUserId(user);
            return userId!;
        }
    }
}
