using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_153505_Shevtsova_D.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly Services.ProductService.IProductService _context;
        public DeleteModel(Services.ProductService.IProductService context)
        {
            _context = context;
        }

        [BindProperty]
        public Tea Tea { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tea = await _context.GetProductByIdAsync(id ?? default(int));

            if (tea == null)
            {
                return NotFound();
            }
            else 
            {
                Tea = tea.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _context.DeleteProductAsync(id ?? default(int));

            return RedirectToPage("./Index");
        }
    }
}

