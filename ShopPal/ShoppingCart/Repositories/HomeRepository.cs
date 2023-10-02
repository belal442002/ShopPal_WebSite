using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(String searchFor ="", int categoryId=0)
        {
            searchFor = searchFor.ToLower();
            IEnumerable<Product> products = await (from product in _db.Products
                                             join category in _db.Categories
                                             on product.CategoryId equals category.Id
                                             where String.IsNullOrWhiteSpace(searchFor) || (product != null && product.Name.StartsWith(searchFor))
                                             select new Product
                                             {
                                                 Id = product.Id,
                                                 Name = product.Name,
                                                 CategoryId = product.CategoryId,
                                                 CategoryName = category.Name,
                                                 Description = product.Description,
                                                 Price = product.Price,
                                                 Image = product.Image
                                             }).ToListAsync();
            if(categoryId > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId).ToList();
            }
            return products;
        }
    }
}
