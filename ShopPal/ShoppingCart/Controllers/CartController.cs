using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<IActionResult> AddItem(int ProductId, int quantity=1, int redirect=0)
        {
            var cartCount = await _cartRepository.AddItem(ProductId, quantity);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int ProductId)
        {
            var cartCount = await _cartRepository.RemoveItem(ProductId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> CartItemCount()
        {
            var cartCount = await _cartRepository.ShoppingCartItems();
            return Ok(cartCount);
        }

    public async Task<IActionResult> DoCheckOut()
        {
            bool isCheckedOut = await _cartRepository.DoCheckout();
            if (!isCheckedOut)
                throw new Exception("Somthing went wrong");
            return RedirectToAction("Index", "Home");
        }
    }
}
