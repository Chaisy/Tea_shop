using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;

namespace Web_153505_Shevtsova_D.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Services.ProductService.IProductService _context;

        public IndexModel(Services.ProductService.IProductService context)
        {
            _context = context;
        }

        public ListModel<Tea> Tea { get;set; } = default!;

        public async Task OnGetAsync(int pageno)
        {
            var answer = (await _context.GetProductListAsync(null, pageno));
            if (answer.Success)
            {
                Tea = answer.Data;
            }
        }
    }
}