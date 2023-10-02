using ShoppingCart.Models;

namespace ShoppingCart.ViewModels
{
    public class DisplayAllProductsHome
    {
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public String SearchFor { get; set; } = "";
        public int CategoryId { get; set; } = 0;
    }
}
