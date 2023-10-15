using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;
using Web_153505_Shevtsova_D.Services.TeaBasesService;


namespace Web_153505_Shevtsova_D.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        List<Tea> _teas;
        List<TeaBasesCategory> _engineTypes;
        IConfiguration _config;

        public MemoryProductService([FromServices] IConfiguration config,
                                            ICategoryService categoryService)
        {
            _engineTypes = categoryService.GetCategoryListAsync().Result.Data;
            _config = config;
            SetupData();
        }

        // формирует данные предметной области
        private void SetupData()
        {
            _teas = new List<Tea> 
            {
                new Tea
                {
                    Id=1,
                    Name="Taiga glade",
                    Description="hibiscus, apple pieces, pine cones, juniper berries, blackberries, red currants, " +
                                "blueberries, cornflower petals, aroma of grandma's jam",
                    Category= _engineTypes.Find(c => c.NormalizedName.Equals("frukti"))!,
                    Price=15,
                    PhotoPath="Images/taiga.jpeg",
                },
                new Tea
                {
                    Id=2,
                    Name="Lavender apple",
                    Description="Pieces of apples, linden, lemon balm, blackberry leaves, lavender, chamomile.",
                    Category= _engineTypes.Find(c => c.NormalizedName.Equals("listia"))!,
                    Price=17,
                    PhotoPath="Images/lavenderapple.jpeg",
                },
                new Tea
                {
                    Id=3,
                    Name="Lapacho",
                    Description="tincture of Lapacho tree bark, sugar, honey",
                    Category= _engineTypes.Find(c => c.NormalizedName.Equals("kora"))!,
                    Price=20,
                    PhotoPath="Images/lapacho.jpeg",
                },
                new Tea
                {
                    Id=4,
                    Name="Peach Tea",
                    Description="peaches, herbal lemon balm, granulated sugar or a sugar ",
                    Category= _engineTypes.Find(c => c.NormalizedName.Equals("frukti"))!,
                    Price=15,
                    PhotoPath="Images/peachtea.jpeg",
                },
                 new Tea{
                    Id =5,
                    Name = "Matcha",
                    Description = "Japanese green tea powder",
                    Category = _engineTypes.Find(c => c.NormalizedName.Equals("listia"))!,
                    Price = 22,
                    PhotoPath = "Images/matcha.jpeg",
                },
                new Tea
                {
                    Id = 6,
                    Name = "Buddha Basket (with marigold flower)",
                    Description = "This bound tea is made from tea leaves bound with calendula flowers. With orange and peach aroma.",
                    Category = _engineTypes.Find(c => c.NormalizedName.Equals("tsveti"))!,
                    Price = 10,
                    PhotoPath = "Images/budda.jpeg",
                },
                new Tea
                {
                    Id = 7,
                    Name = "Masala (spiced tea)",
                    Description = "Fennel, cardamom, cinnamon, cumin, ginger, cloves, coriander, white, black and pink pepper",
                    Category = _engineTypes.Find(c => c.NormalizedName.Equals("korni"))!,
                    Price = 16,
                    PhotoPath = "Images/masala.jpeg",
                },
                new Tea
                {
                    Id = 8,
                    Name = "Black Tea",
                    Description = "Just leaves",
                    Category = _engineTypes.Find(c => c.NormalizedName.Equals("listia"))!,
                    Price = 12,
                    PhotoPath = "Images/blacktea.jpeg",
                },


            };
        }

        public Task<ResponseData<Tea>> CreateProductAsync(Tea product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Tea>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Tea>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // объект ответа
            var response = new ResponseData<ListModel<Tea>>();
            // данный которые засунутся в объект ответа
            ListModel<Tea> listModel = new ListModel<Tea>();

            var pageSize = int.Parse(_config["ItemsPerPage"]!);

           // проверка на допуститмость номера страницы
            
            //иф чтобы учесть плюс одну страницу, если она не полностью заполнена
            if (pageNo*pageSize - _teas.Count > pageSize)
            {
                throw new Exception("page number are greater then amount of pages");
            }

            // filteredTeass вынесен в отдельную переменную, только чтобы получить общее количество
            // чаев соответствующих данной категории ( для корректного отображения номеров страниц)
            var filteredTeass = _teas.
                Where(d => categoryNormalizedName == null ||
                    d.Category.NormalizedName.Equals(categoryNormalizedName)); // фильтр по категории
                
            listModel.Items = filteredTeass.Skip((pageNo - 1) * 3). // пропускаем элементы, которые не будут отображены
                Take(pageSize). // выбираем столько, сколько поместится на страницу
                ToList(); // конвертируем в список


            // округляем в большую сторону чтобы поместились все элементы
            var totalPages = Math.Ceiling((double)filteredTeass.Count() / (double)pageSize);
            listModel.TotalPages = (int)totalPages;
            listModel.CurrentPage = pageNo;

            // если нет чаев соответствующих данной категории
            // сообщаем об ошибке
            if (listModel.Items.Count == 0)
            {
                response.Success = false;
                response.ErrorMessage = "can't find tea with such base type";
            }

            // заносим данные в объект ответа
            response.Data = listModel;
            return Task.FromResult(response);
        }

        public Task UpdateProductAsync(int id, Tea product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}

