using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_153505_Shevtsova_D.Domain.Models;
using Web_153505_Shevtsova_D.Services.ProductService;

namespace Web_153505_Shevtsova_D.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly Cart _cart;

        public CartController(IProductService productService, Cart cart)
        {
            _productService = productService;
            _cart = cart;
        }

        [Authorize]
        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string ReturnUrl)
        {
            var response = (await _productService.GetProductByIdAsync(id));
            if (response.Success)
                _cart.AddToCart(response.Data);
            return Redirect(ReturnUrl);
        }

        [Authorize]
        [Route("[controller]/")]
        public async Task<ActionResult> Index()
        {
            return View(_cart.CartItems);
        }

        [Authorize]
        [Route("[controller]/delete/{id:int}")]
        public async Task<ActionResult> Delete(int id, string ReturnUrl)
        {
            _cart.RemoveItems(id);
            return Redirect(ReturnUrl);
        }

    }
}