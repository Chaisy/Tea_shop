using Microsoft.AspNetCore.Mvc;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;
using Web_153505_Shevtsova_D.Services.TeaBasesService;
using Web_153505_Shevtsova_D.Services.ProductService;

namespace Web_153505_Shevtsova_D.Controllers
{
    public class TeaProductController : Controller
    {
        readonly IProductService _productService;
        readonly ICategoryService _categoryService;


        public TeaProductController(IProductService productService, ICategoryService categoryService)
        {
            this._productService = productService;
            this._categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category, int pageno)
        {
            // получаем список чаев
            var productResponse = 
                await _productService.GetProductListAsync(category, pageno);

            // обработка неуспешного обращения
            if (!productResponse.Success)
                return NotFound(productResponse.ErrorMessage);

            // получаем все категории
            var categories = _categoryService.GetCategoryListAsync().Result.Data;

            // currentCategory вынесено в отдельную переменную, чтобы проверить на null и, в случае null, передать 
            // в представление строку "Все"
            var currentCategory = categories.Find(c => c.NormalizedName == category);

            // чтобы отображать категорию "все", на странице с типами
            ViewData["currentCategory"] = currentCategory == null ? "ALL" : currentCategory.Name;
            ViewBag.categories = categories;

            return View(productResponse.Data);
        }
    }
}