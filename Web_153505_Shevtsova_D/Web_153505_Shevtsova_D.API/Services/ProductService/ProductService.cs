/*using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Web_153505_Shevtsova_D.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        AppDbContext _context;
        private readonly int _maxPageSize;
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor accessor;

        public ProductService(AppDbContext context, ConfigurationManager configurationManager,
            ConfigurationService configurationService, IWebHostEnvironment env,
            IHttpContextAccessor accessor)
        {
            _context = context;

            _maxPageSize = Convert.ToInt32(configurationManager["MaxPageSize"]);
            this.env = env;
            this.accessor = accessor;

        }
          public async Task<ResponseData<Tea>> CreateProductAsync(Tea product)
        {
            // получаем существующую категорию
            // надо, потому что без этого блока, при передаче id категории в теле запроса
            // бросало исключение
            product.Category = _context.basesType.Where(c => c.NormalizedName == product.Category.NormalizedName).FirstOrDefault()!;
            if (product.Category == null)
                throw new Exception("невозможно найти категорию при добвалении товара");
            await _context.teas.AddAsync(product);
            _context.SaveChanges();
            return new ResponseData<Tea> { Data = product};
        }
        public async Task DeleteProductAsync(int id)
        {
            var tea = await _context.teas.FindAsync(id);
            
            if (tea != null)
                _context.teas.Remove(tea);
            _context.SaveChanges();
        }
        public async Task<ResponseData<Tea>> GetProductByIdAsync(int id)
        {
            var query = _context.teas.AsQueryable();
           
            var data = await query.Where(p => p.Id == id).Include(p => p.Category).FirstOrDefaultAsync();
            var response = new ResponseData<Tea>();
            if (data != null)
                 response.Data = data;
            else
            {
                response.ErrorMessage = "can't find such tea";
                response.Success = false;
            }
            return response;
        }
        public async Task<ResponseData<ListModel<Tea>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize= _maxPageSize;
            // объект ответа
            var response = new ResponseData<ListModel<Tea>>();
            // данный которые засунутся в объект ответа
            ListModel<Tea> listModel = new ListModel<Tea>();
            var query = _context.teas.AsQueryable();
            // запрос для получения самолетов по категориям
            query = query
             .Where(d => categoryNormalizedName == null
             || d.Category.NormalizedName.Equals(categoryNormalizedName)).Include(p => p.Category);
            // количество элементов в списке
            var count = query.Count();
            if (count == 0)
            {
                response.ErrorMessage = "no teas founded";
                response.Success = false;
                response.Data = listModel;
                return response;
            }
            // проверка на допуститмость номера страницы
            // 
            // иф такой хитрый, чтобы учесть плюс одну страницу, если она не полностью заполнена
            if (pageNo * pageSize - count > pageSize)
            {
                throw new Exception("page number are greater then amount of pages");
            }
            listModel.Items = await query.Skip((pageNo - 1) * 3). // пропускаем элементы, которые не будут отображены
                Take(pageSize). // выбираем столько, сколько поместится на страницу
                ToListAsync(); // конвертируем в список
            // округляем в большую сторону чтобы поместились все элементы
            listModel.TotalPages = (int)Math.Ceiling((double)count / (double)pageSize);
            listModel.CurrentPage = pageNo;
            // если нет самолетов соответствующих данной категории
            // сообщаем об ошибке
            if (listModel.Items.Count == 0)
            {
                response.Success = false;
                response.ErrorMessage = "can't find teas with such base type";
            }
            // заносим данные в объект ответа
            response.Data = listModel;
            return response;
        }
        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            // ищем самолет по id
            var responseData = await GetProductByIdAsync(id);
            Tea tea;
            if (!responseData.Success)
            {
                return new ResponseData<string>
                { Success = false, ErrorMessage = responseData.ErrorMessage };
            }
            else
                tea = responseData.Data;
            
            var host = "https://" + accessor.HttpContext!.Request.Host;

            var imageFolder = Path.Combine(env.WebRootPath, "Images");
            
            if (formFile != null)
            {
                // удаляем предыдущее изображение
                if (!String.IsNullOrEmpty(tea.PhotoPath))
                {
                    // получаем путь предыдущего изображения как путь к папке с изображеним +
                    // имя и расширение самого изображения
                    var prevImage = Path.Combine(imageFolder, Path.GetFileName(tea.PhotoPath));
                    File.Delete(prevImage);

                    // или File.Delete(tea.PhotoPath);
                }

                // Создать имя файла
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                // получаем путь куда сохранять фото
                var filePath = Path.Combine(imageFolder, fName);

                // Сохранить файл
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                tea.PhotoPath = $"{host}/Images/{fName}";
                
                await _context.SaveChangesAsync();
                return new ResponseData<string>
                {
                    Data = tea.PhotoPath
                };
            }
            else
            { return new ResponseData<string> { Success = false, ErrorMessage = "Error: no file where provided" }; }
        }

        
        // в методе update может возникнуть исключение при удалении
        // объекта в момент его изменения. Где его обрабатывать?
        public async Task UpdateProductAsync(int id, Tea product)
        {
            // ищем самолет по id
            var responseData = await GetProductByIdAsync(id);
            Tea tea;
            if (responseData.Success)
            {
                tea = responseData.Data;
                // наичнаем отслеживать найденный самолет и
                // переводим его в состояние Modified
                _context.teas.Update(tea);
                tea.Price = product.Price;
                tea.Category = _context.basesType.Where(c => c.Id == product.Category.Id).First();
                tea.PhotoPath = product.PhotoPath;
                tea.Description = product.Description;
                tea.Name = product.Name;
                // попробовать раскоментить эту штуку и закоментить все остальное
                //_context.Attach(tea).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    


        private void DeleteImage(string imageFolder, Tea tea)
        {
            // удаляем предыдущее изображение
            //
            // стоит делать проверку на null в этом методе
            // или об этом должен позаботиться вызывающий метод?
            if (!String.IsNullOrEmpty(tea.PhotoPath))
            {
                // получаем путь предыдущего изображения как путь к папке с изображеним +
                // имя и расширение самого изображения
                var prevImage = Path.Combine(imageFolder, Path.GetFileName(tea.PhotoPath));
                File.Delete(prevImage);

                // или File.Delete(tea.PhotoPath);
            }
        }
        // в методе update может возникнуть исключение при удалении
        // объекта в момент его изменения. Где его обрабатывать?
        /*public async Task UpdateProductAsync(int id, Tea product)
        {
            // ищем самолет по id
            var responseData = await GetProductByIdAsync(id);
            Tea tea;

            if (responseData.Success)
            {
                tea = responseData.Data;

                // наичнаем отслеживать найденный самолет и
                // переводим его в состояние Modified
                _context.teas.Update(tea);

                tea.Price = product.Price;
                tea.Category = _context.basesType.Where(c => c.Id == product.Category.Id).First();
                tea.PhotoPath = product.PhotoPath;
                tea.Description = product.Description;
                tea.Name = product.Name;

                // попробовать раскоментить эту штуку и закоментить все остальное
                //_context.Attach(tea).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
        }#1#
    }
}*/

using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.API.Services;
using Web_153505_Shevtsova_D.API.Services.ProductService;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;
namespace Web_153505_Shevtsova_D.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        AppDbContext _context;
        private readonly int _maxPageSize;

        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor accessor;
        
        public ProductService(AppDbContext context, ConfigurationManager configurationManager,
                            ConfigurationService configurationService, IWebHostEnvironment env,
                            IHttpContextAccessor accessor)
        {
            _context = context;


            _maxPageSize = Convert.ToInt32(configurationManager["MaxPageSize"]);
            this.env = env;
            this.accessor = accessor;

        }

        public async Task<ResponseData<Tea>> CreateProductAsync(Tea product)
        {
            // получаем существующую категорию
            // надо, потому что без этого блока, при передаче id категории в теле запроса
            // бросало исключение
            product.Category = _context.basesType.Where(c => c.NormalizedName == product.Category.NormalizedName).FirstOrDefault()!;
            if (product.Category == null)
                throw new Exception("невозможно найти категорию при добвалении товара");
            await _context.teas.AddAsync(product);
            _context.SaveChanges();
            return new ResponseData<Tea> { Data = product, Success = true, ErrorMessage = ""};
        }
        public async Task DeleteProductAsync(int id)
        {
            var tea = await _context.teas.FindAsync(id);
            
            if (tea != null)
                _context.teas.Remove(tea);
            _context.SaveChanges();
        }
        public async Task<ResponseData<Tea>> GetProductByIdAsync(int id)
        {
            var query = _context.teas.AsQueryable();
           
            var data = await query.Where(p => p.Id == id).Include(p => p.Category).FirstOrDefaultAsync();
            var response = new ResponseData<Tea>();
            if (data != null)
                 response.Data = data;
            else
            {
                response.ErrorMessage = "can't find such tea";
                response.Success = false;
            }
            return response;
        }
        public async Task<ResponseData<ListModel<Tea>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize= _maxPageSize;
            // объект ответа
            var response = new ResponseData<ListModel<Tea>>();
            // данный которые засунутся в объект ответа
            ListModel<Tea> listModel = new ListModel<Tea>();
            var query = _context.teas.AsQueryable();
            // запрос для получения самолетов по категориям
            query = query
             .Where(d => categoryNormalizedName == null
             || d.Category.NormalizedName.Equals(categoryNormalizedName)).Include(p => p.Category);
            // количество элементов в списке
            var count = query.Count();
            if (count == 0)
            {
                response.ErrorMessage = "no teas founded";
                response.Success = false;
                response.Data = listModel;
                return response;
            }
            // проверка на допуститмость номера страницы
            // 
            // иф такой хитрый, чтобы учесть плюс одну страницу, если она не полностью заполнена
            if (pageNo * pageSize - count > pageSize)
            {
                throw new Exception("page number are greater then amount of pages");
            }
            listModel.Items = await query.Skip((pageNo - 1) * 3). // пропускаем элементы, которые не будут отображены
                Take(pageSize). // выбираем столько, сколько поместится на страницу
                ToListAsync(); // конвертируем в список
            // округляем в большую сторону чтобы поместились все элементы
            listModel.TotalPages = (int)Math.Ceiling((double)count / (double)pageSize);
            listModel.CurrentPage = pageNo;
            // если нет самолетов соответствующих данной категории
            // сообщаем об ошибке
            if (listModel.Items.Count == 0)
            {
                response.Success = false;
                response.ErrorMessage = "can't find teas with such engine type";
            }
            // заносим данные в объект ответа
            response.Data = listModel;
            return response;
        }
        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            // ищем  по id
            var responseData = await GetProductByIdAsync(id);
            Tea tea;
            
            if (!responseData.Success)
            {
                return new ResponseData<string>
                { Success = false, ErrorMessage = responseData.ErrorMessage };
            }
            else
                tea = responseData.Data;


           
            var host = "https://" + accessor.HttpContext!.Request.Host;

            var imageFolder = Path.Combine(env.WebRootPath, "Images");



            if (formFile != null)
            {

                // удаляем предыдущее изображение
                if (!String.IsNullOrEmpty(tea.PhotoPath))
                {
                    // получаем путь предыдущего изображения как путь к папке с изображеним +
                    // имя и расширение самого изображения
                    var prevImage = Path.Combine(imageFolder, Path.GetFileName(tea.PhotoPath));
                    File.Delete(prevImage);

                    // или File.Delete(tea.PhotoPath);
                }

                // Создать имя файла
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                // получаем путь куда сохранять фото
                var filePath = Path.Combine(imageFolder, fName);

                // Сохранить файл
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                tea.PhotoPath = $"https://localhost:7002/Images/{fName}";
                tea.MIMEType = ext;


                await _context.SaveChangesAsync();
                return new ResponseData<string>
                {
                    Data = tea.PhotoPath
                };
            }
            else
            { return new ResponseData<string> { Success = false, ErrorMessage = "Error: no file where provided" }; }
        }

        // в методе update может возникнуть исключение при удалении
        // объекта в момент его изменения. Где его обрабатывать?
        public async Task UpdateProductAsync(int id, Tea product)
        {
            // ищем самолет по id
            var responseData = await GetProductByIdAsync(id);
            Tea tea;
            if (responseData.Success)
            {
                tea = responseData.Data;
                // наичнаем отслеживать найденный самолет и
                // переводим его в состояние Modified
                _context.teas.Update(tea);
                tea.Price = product.Price;
                tea.Category = _context.basesType.Where(c => c.Id == product.Category.Id).First();
                tea.PhotoPath = product.PhotoPath;
                tea.Description = product.Description;
                tea.Name = product.Name;
                // попробовать раскоментить эту штуку и закоментить все остальное
                //_context.Attach(tea).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}

