using Microsoft.AspNetCore.Mvc;
using SanalMarket.Infrastructure.Abstract;
using SanalMarket.Infrastructure.Models;

namespace SanalMarket.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }
        
        public IActionResult Index()
        {   
            var cart = _cartService.GetMyCart();
            var model = new CartIndexViewModel()
            {
                Carts = cart,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int productId)
        {
            var product = _productService.GetById(productId);
            if (product == null)
                return NotFound();

            var cartItem = new CartItem
            {
                Product = product,
                Quantity = 1
            };

            _cartService.Add(cartItem);

            return ViewComponent("CartSummary");
        }
        public IActionResult Remove(int productId)
        {
            _cartService.Remove(productId);
            var model = new CartIndexViewModel()
            {
                Carts = _cartService.GetMyCart()
            };
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.Remove(productId);

            return RedirectToAction("Index", "Product");
        }
    }
}
