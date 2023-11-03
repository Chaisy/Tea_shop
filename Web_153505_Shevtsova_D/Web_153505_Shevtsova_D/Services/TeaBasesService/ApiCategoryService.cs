using System.Text;
using Newtonsoft.Json;
using Web_153505_Shevtsova_D.Services.TeaBasesService;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;
using Web_153505_Shevtsova_D.Services.ProductService;

namespace Web_153505_Shevtsova_D.Services
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiProductService> _logger;

        public ApiCategoryService(HttpClient httpClient,
            ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<ResponseData<List<TeaBasesCategory>>> GetCategoryListAsync()
        {
            // подготовка URL запроса
            var urlString= new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}TeaBasesCategories/");
            Console.WriteLine(urlString);

            // отправить запрос к API
            var response = await _httpClient.GetAsync(
                new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    //#pragma warning disable CS8603 // Possible null reference return.
                    var content = response.Content;
                    return await response.Content.ReadFromJsonAsync<ResponseData<List<TeaBasesCategory>>>();
                    //#pragma warning restore CS8603 // Possible null reference return.

                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<List<TeaBasesCategory>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return new ResponseData<List<TeaBasesCategory>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
            };
        }
    }
}