using Web_153505_Shevtsova_D.Domain.Models;
using Web_153505_Shevtsova_D.Domain.Entities;
namespace Web_153505_Shevtsova_D.Services.TeaBasesService;

public interface ICategoryService
{
    /// <summary>
    /// Получение списка всех категорий
    /// </summary>
    /// <returns></returns>
    public Task<ResponseData<List<TeaBasesCategory>>> GetCategoryListAsync();
}