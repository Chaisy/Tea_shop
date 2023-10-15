using Microsoft.AspNetCore.Mvc;

namespace Web_153505_Shevtsova_D.Components
{
    public class Cart: ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();

        }
        
    }
}
