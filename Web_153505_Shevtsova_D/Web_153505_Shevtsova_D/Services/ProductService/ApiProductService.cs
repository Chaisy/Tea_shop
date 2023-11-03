/*using System.Net.Http;
using System.Text;
using System.Text.Json;
using Web_153505_Shevtsova_D.API.Services.ProductService;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;

namespace Web_153505_Shevtsova_D.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;

        public ApiProductService(HttpClient httpClient,
                                 IConfiguration configuration,
                                 ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
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

        public async Task<ResponseData<ListModel<Tea>>> GetProductListAsync(
                                         string? categoryNormalizedName,
                                         int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}teas/");
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            };
            // добавить номер страницы в маршрут
            if (pageNo > 1)
            {
                urlString.Append($"pageno{pageNo}");
            };
            // добавить размер страницы в строку запроса
            if (!_pageSize!.Equals("3"))
            {
                urlString.Append(QueryString.Create("pagesize", _pageSize));
            }

            // отправить запрос к API
            var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Tea>>>(_serializerOptions);
#pragma warning restore CS8603 // Possible null reference return.

                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Tea>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: { response.StatusCode.ToString()}");
                     return new ResponseData<ListModel<Tea>>
                     {
                         Success = false,
                         ErrorMessage = $"Данные не получены от сервера. Error: { response.StatusCode.ToString() }"
                     };
        }


        public Task UpdateProductAsync(int id, Tea product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}*/


using System.Text;
using System.Text.Json;
using Web_153505_Shevtsova_D.Domain.Entities;
using Web_153505_Shevtsova_D.Domain.Models;
namespace Web_153505_Shevtsova_D.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;
        public ApiProductService(HttpClient httpClient,
                                 IConfiguration configuration,
                                 ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }


        public async Task<ResponseData<Tea>> CreateProductAsync(Tea product, IFormFile? formFile)
        {
            
            var urlString
            = new
            StringBuilder("http://localhost:5002/api/teas/");

            var response = await _httpClient.PostAsJsonAsync(new Uri(urlString.ToString()), product, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var plane = await response.Content.ReadFromJsonAsync<Tea>();

                    if (formFile != null)
                        await SaveImageAsync(plane!.Id, formFile);

                    return new ResponseData<Tea> { Data = plane!, Success = true, ErrorMessage = ""};
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return  new ResponseData<Tea>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            else
            {
                _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
                return new ResponseData<Tea>
                {
                    Success = false,
                    ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
                };

            }
        }


        public async Task DeleteProductAsync(int id)
        {
            
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}teas/{id}");

            // отправить запрос к API
            var response = await _httpClient.DeleteAsync(
            new Uri(urlString.ToString()));

        }


        public async Task<ResponseData<Tea>> GetProductByIdAsync(int id)
        {
            
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}teas/{id}");

            // отправить запрос к API
            var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    #pragma warning disable CS8603 // Possible null reference return.
                    var content = response.Content;
                    return await response.Content.ReadFromJsonAsync<ResponseData<Tea>>()!;
                    #pragma warning restore CS8603 // Possible null reference return.

                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Tea>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return new ResponseData<Tea>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode.ToString()}"
            };

        }

        public async Task<ResponseData<ListModel<Tea>>> GetProductListAsync(
                                         string? categoryNormalizedName,
                                         int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString
            = new
            StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}teas/");
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            };
            // добавить номер страницы в маршрут
            if (pageNo > 1)
            {
                urlString.Append($"pageno{pageNo}");
            };
            // добавить размер страницы в строку запроса
            if (!_pageSize!.Equals("3"))
            {
                urlString.Append(QueryString.Create("pagesize", _pageSize));
            }
            // отправить запрос к API
            
            
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    //#pragma warning disable CS8603 // Possible null reference return.
                    var content = response.Content;     
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Tea>>>();
//#pragma warning restore CS8603 // Possible null reference return.
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Tea>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: { response.StatusCode.ToString()}");
                     return new ResponseData<ListModel<Tea>>
                     {
                         Success = false,
                         ErrorMessage = $"Данные не получены от сервера. Error: { response.StatusCode.ToString() }"
                     };
        }



        public async Task UpdateProductAsync(int id, Tea product, IFormFile? formFile)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}teas/{id}");

            var response = await _httpClient.PutAsJsonAsync(new Uri(urlString.ToString()), product);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var plane = await response.Content.ReadFromJsonAsync<Tea>();

                    if (formFile != null)
                        await SaveImageAsync(plane!.Id, formFile);

                    return;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri
            ($"{_httpClient.BaseAddress.AbsoluteUri}Teas/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent =
            new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;
            var answ = await _httpClient.SendAsync(request);
            if (!answ.IsSuccessStatusCode)
                throw new Exception("Couldnt save image. Probably wrong plane id");
        }
    }
}