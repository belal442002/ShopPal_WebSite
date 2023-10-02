using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Controllers
{
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _userOrder;

        public UserOrderController(IUserOrderRepository userOrder)
        {
            _userOrder = userOrder;
        }
        public async Task<IActionResult> UserOrders()
        {
            var orders = await _userOrder.GetUserOrders();
            return View(orders);
        }
    }
}
