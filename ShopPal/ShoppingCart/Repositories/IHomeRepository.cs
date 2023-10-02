using ShoppingCart.Models;

namespace ShoppingCart
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<Product>> GetProductsAsync(String searchFor = "", int categoryId = 0);
    }
}