using ShoppingCart.Models;

namespace ShoppingCart
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> GetUserOrders();
    }
}