using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;
using System.Diagnostics;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(String SearchFor="", int CategoryId=0)
        {
            IEnumerable<Product> products = await _homeRepository.GetProductsAsync(SearchFor, CategoryId);
            IEnumerable<Category> categories = await _homeRepository.GetCategoriesAsync();
            DisplayAllProductsHome display = new DisplayAllProductsHome
            {
                products = products,
                categories = categories,
                SearchFor = SearchFor,
                CategoryId = CategoryId
            };
            return View(display);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}