using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _admin;

        public AdminController(IAdminRepository admin)
        {
            _admin = admin;
        }
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            Add_EditViewModel model = new Add_EditViewModel();
            model.categories = await _admin.GetCategories();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile? ImageFile = null)
        {
            var pr = _admin.UploadAndSetImage(product, ImageFile);
            Add_EditViewModel model = new Add_EditViewModel();
            model.product = pr;
            model.categories = await _admin.GetCategories();
            if(!ModelState.IsValid)
                return View(model);
            var added = _admin.AddProduct(pr);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int ProductId)
        {
            Add_EditViewModel model = new Add_EditViewModel();

            var product = await _admin.GetProduct(ProductId);
            var categories = await _admin.GetCategories();

            model.product = product;
            model.categories = categories;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile? ImageFile = null)
        {
            var pr = _admin.UploadAndSetImage(product, ImageFile!);
            if (!ModelState.IsValid)
            {
                Add_EditViewModel model = new Add_EditViewModel();
                var categories = await _admin.GetCategories();

                model.product = pr;
                model.categories = categories;

                return View(model);
            }

            var edited = _admin.Edit(pr);
            return RedirectToAction("Index", "Home");
            
        }

        public IActionResult Delete(int ProductId)
        {
            var deleted = _admin.Delete(ProductId);
            return RedirectToAction("Index", "Home");
        }



    }

    
}
