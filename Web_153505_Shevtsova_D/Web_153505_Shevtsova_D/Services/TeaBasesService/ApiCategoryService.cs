using Web_153505_Shevtsova_D.API.Services.CategoryService;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;

namespace WEB_153501_BYCHKO.Services.EngineTypeCategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        public Task<ResponseData<List<TeaBasesCategory>>> GetCategoryListAsync()
        {
            throw new NotImplementedException();
        }
    }
}