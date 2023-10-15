using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;

namespace Web_153505_Shevtsova_D.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        readonly AppDbContext _appDbContext;
        public CategoryService(AppDbContext db) 
        {
            _appDbContext = db;
        }
        public async Task<ResponseData<List<TeaBasesCategory>>> GetCategoryListAsync()
        {
            return new ResponseData<List<TeaBasesCategory>> { Data = await _appDbContext.basesType.ToListAsync() };
        }
    }
}

