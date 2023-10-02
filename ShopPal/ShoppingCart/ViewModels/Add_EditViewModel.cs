using ShoppingCart.Models;

namespace ShoppingCart.ViewModels
{
    public class Add_EditViewModel
    {
        public Product product { get; set; }
        public List<Category> categories { get; set; }
    }
}
