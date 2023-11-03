using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.Domain.Entities;

namespace Web_153505_Shevtsova_D.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Services.ProductService.IProductService _context;

        public IndexModel(Services.ProductService.IProductService context)
        {
            _context = context;
        }

        public IList<Tea> Tea { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var answer = (await _context.GetProductListAsync(null));
            if (answer.Success)
            {
                Tea = answer.Data.Items;
            }
        }
    }
}