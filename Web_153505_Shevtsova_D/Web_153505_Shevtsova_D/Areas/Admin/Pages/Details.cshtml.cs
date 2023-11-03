using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153505_Shevtsova_D.Domain.Entities;


namespace Web_153505_Shevtsova_D.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly Services.ProductService.IProductService _context;

        public DetailsModel(Services.ProductService.IProductService context)
        {
            _context = context;
        }

        public Tea Tea { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tea = await _context.GetProductByIdAsync(id ?? default(int));
            if (tea.Success == false)
            {
                return NotFound();
            }
            else 
            {
                Tea = tea.Data;
            }
            return Page();
        }
    }
}
