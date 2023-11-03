using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Services.ProductService;
using Web_153505_Shevtsova_D.Services.TeaBasesService;

namespace Web_153505_Shevtsova_D.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _context;
        private readonly ICategoryService _service;
        
        [BindProperty]
        public IFormFile? Image { get; set; }

        public EditModel(Services.ProductService.IProductService context,
                         Services.TeaBasesService.ICategoryService service)
        {
            _context = context;
            _service = service;
        }

        [BindProperty]
        public Tea Tea { get; set; } = default!;

        [BindProperty]
        public int CategoryId { get; set; }

        public SelectList selectList { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tea =  await _context.GetProductByIdAsync(id ?? default(int));
            if (tea.Success == false)
            {
                return NotFound();
            }

            Tea = tea.Data;
            selectList = new SelectList((await _service.GetCategoryListAsync()).Data,
                            nameof(TeaBasesCategory.Id), nameof(TeaBasesCategory.Name), Tea.Category!.Name);


            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Tea.Category = (await _service.GetCategoryListAsync()).Data.Where(c => c.Id == CategoryId).FirstOrDefault();

            ModelState.ClearValidationState(nameof(Tea));

            var mm = TryValidateModel(Tea, nameof(Tea));
            
            if (!TryValidateModel(Tea, nameof(Tea)))
            {
                selectList = new SelectList((await _service.GetCategoryListAsync()).Data,
                                        nameof(TeaBasesCategory.Id), nameof(TeaBasesCategory.Name));
                return Page();
            }

            await _context.UpdateProductAsync(Tea.Id, Tea, Image);

            return RedirectToPage("./Index");
        }
    }
}