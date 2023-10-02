using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminRepository(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Product> GetProduct(int ProductId)
        {
            var product = await (from p in _dbContext.Products
                           join c in _dbContext.Categories
                           on p.CategoryId equals c.Id
                           where p.Id == ProductId
                           select new Product
                           {
                               Id = p.Id,
                               Name = p.Name,
                               Description = p.Description,
                               Price = p.Price,
                               Image = p.Image,
                               CategoryId = p.CategoryId,
                               CategoryName = c.Name,

                           }).FirstOrDefaultAsync();
            return product!;
        }

        public  bool Edit(Product product)
        {
            try
            {
                _dbContext.Products.Update(product);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int ProductId)
        {
            try
            {
                var product = _dbContext.Products.Find(ProductId);
                _dbContext.Products.Remove(product!);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool AddProduct(Product product)
        {
            try
            {
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Category>> GetCategories()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            if (categories.Count == 0)
                throw new Exception("No categories in db");
            return categories;
        }

        public Product UploadAndSetImage(Product product, IFormFile? ImageFile = null)
        {
            try
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Generate a unique file name for the image
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);

                    // Define the path to save the image in your 'images' folder
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    // Save the image file name in the database
                    product.Image = fileName;
                }

                return product;
            }
            catch (Exception)
            {
                throw new Exception("Somthing went wrong");
            }
        }
    }
}
 