using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;


namespace Web_153505_Shevtsova_D.Services.TeaBasesService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<TeaBasesCategory>>> GetCategoryListAsync()
        {
            var categories = new List<TeaBasesCategory> 
            {
                new TeaBasesCategory{ Id = 1, Name="Leaves", NormalizedName="listia"},
                new TeaBasesCategory{ Id = 2, Name="Roots", NormalizedName="korni"},
                new TeaBasesCategory{ Id = 3, Name="Flowers", NormalizedName="tsveti"},
                new TeaBasesCategory{ Id = 4, Name="Bark", NormalizedName="kora"},
                new TeaBasesCategory{ Id = 5, Name="Fruits", NormalizedName="frukti"},
            };

            // создание объекта ответа и передача данных в него
            var result = new ResponseData<List<TeaBasesCategory>>();
            result.Data = categories;

            return Task.FromResult(result);
        }
    }
}