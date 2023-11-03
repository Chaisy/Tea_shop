using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_153505_Shevtsova_D.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly Services.ProductService.IProductService _context;
        private readonly Services.TeaBasesService.ICategoryService _service;


        [BindProperty]
        public IFormFile? Image { get; set; }

        [BindProperty]
        public int CategoryId { get; set; }

        public  SelectList selectList { get; set; }

        [BindProperty]
        public Tea Tea { get; set; } = default!;

        public CreateModel(Services.ProductService.IProductService context,
            Services.TeaBasesService.ICategoryService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<IActionResult> OnGet()
        {
            selectList = new SelectList((await _service.GetCategoryListAsync()).Data, 
                nameof(TeaBasesCategory.Id), nameof(TeaBasesCategory.Name));
            return Page();
        }



        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Tea.Category = (await _service.GetCategoryListAsync()).Data.Where(c => c.Id == CategoryId).FirstOrDefault();
            
            ModelState.ClearValidationState(nameof(Tea));

            var mm = TryValidateModel(Tea, nameof(Tea));
            
            if (!TryValidateModel(Tea, nameof(Tea)) || Tea == null || Image == null)
            {
                selectList = new SelectList((await _service.GetCategoryListAsync()).Data,
                    nameof(TeaBasesCategory.Id), nameof(TeaBasesCategory.Name));
                return Page();
            }

            var response = await _context.CreateProductAsync(Tea, Image);
            if (!response.Success)
                throw new Exception(response.ErrorMessage);

            return RedirectToPage("./Index");
        }
    }
}
