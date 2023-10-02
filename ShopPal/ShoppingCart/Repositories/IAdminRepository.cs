using ShoppingCart.Models;

namespace ShoppingCart
{
    public interface IAdminRepository
    {
        Task<Product> GetProduct(int ProductId);
        Task<List<Category>> GetCategories();
        bool Edit(Product product);
        bool Delete(int ProductId);
        bool AddProduct(Product product);
        Product UploadAndSetImage(Product product, IFormFile? ImageFile = null);

    }
}