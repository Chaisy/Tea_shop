using Microsoft.AspNetCore.Mvc;
using Web_153505_Shevtsova_D.Domain.Models;
using Web_153505_Shevtsova_D.Extensions;
namespace Web_153505_Shevtsova_D.ViewComponents
{
    public class OrderCart : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(GetItems());
        }

        // для иммитации получения данных из какого то места
        private Tuple<int, int> GetItems()
        {

            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new();

            int price = cart.Price;
            int count = cart.Count;
            return new Tuple<int, int>(price, count);
        }
    }
}
