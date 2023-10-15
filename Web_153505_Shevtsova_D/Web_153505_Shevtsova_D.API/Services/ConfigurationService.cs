namespace Web_153505_Shevtsova_D.API.Services
{
    public class ConfigurationService
    {
        // нужен чтобы в контроллере получить доступ
        // к applicationUrl
        public ConfigurationService() 
        {
            var builder = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json"));
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
    }
}

