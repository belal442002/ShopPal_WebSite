using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<int> AddItem(int ProductId, int quantity = 1)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            var userId = GetUserId();
            try
            {
                if (userId is null)
                    throw new Exception("User should logged-in");
                var shoppingCart = await GetCart(userId);
                if (shoppingCart is null)
                {
                    shoppingCart = new Shopping_Cart
                    {
                        UserId = userId
                    };
                    _dbContext.ShoppingCarts.Add(shoppingCart);
                }
                _dbContext.SaveChanges();
                var cart = await _dbContext.CartDetails.FirstOrDefaultAsync(c => c.ShoppingCartId == shoppingCart.Id && c.ProductId == ProductId);
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == ProductId);
                var productPrice = product!.Price;
                if (cart is null)
                {
                    cart = new CartDetail
                    {
                        ShoppingCartId = shoppingCart.Id,
                        ProductId = ProductId,
                        Quantity = quantity,
                        Price = productPrice,
                        TotalPrice = productPrice
                    };
                    _dbContext.CartDetails.Add(cart);
                }
                else
                {
                    cart.Quantity = cart.Quantity + quantity;
                    //cart.TotalPrice = cart.TotalPrice + (cart.Price * quantity);
                    cart.TotalPrice = cart.Price * cart.Quantity ;
                }
                _dbContext.SaveChanges();
                transaction.Commit();
            }
            catch(Exception ex) { 

            }
            return await ShoppingCartItems();
        }

        public async Task<int> RemoveItem(int ProductId)
        {

            {
                var userId = GetUserId();
                try
                {
                    if (userId is null)
                        throw new Exception("User should logged-in");
                    var shoppingCart = await GetCart(userId);
                    if (shoppingCart is null)
                        throw new Exception("cart is empty");
                    
                    var cart = await _dbContext.CartDetails.FirstOrDefaultAsync(c => c.ShoppingCartId == shoppingCart.Id && c.ProductId == ProductId);
                    var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == ProductId);
                    var productPrice = product!.Price;
                    if (cart is null)
                        throw new Exception("Canot remove this item");
                    else if(cart.Quantity == 1)
                    {
                        _dbContext.CartDetails.Remove(cart);
                    }
                    else
                    {
                        cart.Quantity = cart.Quantity - 1;
                        cart.TotalPrice = cart.TotalPrice - cart.Price;
                    }
                    _dbContext.SaveChanges();
                }
                catch (Exception ex)
                {

                }
                return await ShoppingCartItems(userId);
            }

        }

        public async Task<bool> DoCheckout()
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (userId is null)
                    throw new Exception("User not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _dbContext.CartDetails
                                         .Where(c => c.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");

                var order = new Order
                {
                    UserId = userId,
                    OrderStatusId = 1, // pending
                    CreateDate = DateTime.UtcNow,
                };
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();

                foreach(var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        TotalPrice = item.TotalPrice
                    };
                    _dbContext.OrderDetails.Add(orderDetail);
                }
                _dbContext.SaveChanges();

                _dbContext.RemoveRange(cartDetail);
                _dbContext.SaveChanges();
                transaction.Commit();

                return true;
        }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Shopping_Cart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId is null)
                throw new Exception("Invalid user Id");
            var shoppingCart = await _dbContext.ShoppingCarts
                               .Include(a => a.CartDetails)!
                               .ThenInclude(a => a.Product)
                               .ThenInclude(a => a!.Category)
                               .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart!;
        }
        public async Task<int> ShoppingCartItems(String userId="")
        {
            if(String.IsNullOrEmpty(userId))
                 userId = GetUserId();

            if (userId is null)
                throw new Exception("User not logged-in");
            //var cartItems = await (from cart in _dbContext.ShoppingCarts
            //                       join CartDetail in _dbContext.CartDetails
            //                       on cart.Id equals CartDetail.ShoppingCartId
            //                       select new { CartDetail.ShoppingCartId }).ToListAsync();
            //return cartItems.Count;
            var shoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(s => s.UserId == userId);
            if (shoppingCart is null)
                throw new Exception("Cart is empty");
            var items = await _dbContext.CartDetails.Where(c => c.ShoppingCartId == shoppingCart.Id).ToListAsync();
            return items.Count;
        }

        private String GetUserId()
        {
            var user = _httpContextAccessor.HttpContext!.User;
            String userId = _userManager.GetUserId(user)!;
            return userId!;
        }

        private async Task<Shopping_Cart> GetCart(String userId="")
        {
            if (String.IsNullOrEmpty(userId))
                userId = GetUserId();
            var shoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(s => s.UserId == userId);
            return shoppingCart!;

        }
    }
}
