using ShoppingCart.Models;

namespace ShoppingCart
{
    public interface ICartRepository
    {
        Task<int> AddItem(int ProductId, int quantity = 1);
        Task<int> RemoveItem(int ProductId);
        Task<Shopping_Cart> GetUserCart();
        Task<int> ShoppingCartItems(String userId = "");
        Task<bool> DoCheckout();
    }
}