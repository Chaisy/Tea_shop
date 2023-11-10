using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.API.Services;
using Web_153505_Shevtsova_D.API.Models;
using Web_153505_Shevtsova_D.API.Services.CategoryService;
using Web_153505_Shevtsova_D.API.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSingleton(typeof(ConfigurationService));
builder.Services.AddSingleton(builder.Configuration);

builder.Services.Configure<ConfigData>(builder.Configuration.GetSection("ConfigData"));

builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));


builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = false;
        opt.Authority = builder
            .Configuration
            .GetSection("isUri").Value;
        opt.TokenValidationParameters.ValidateAudience = false;
        opt.TokenValidationParameters.ValidTypes =
            new[] { "at+jwt" };
    });

// builder.Services.AddMvc().AddJsonOptions(options =>
// {
//     options.JsonSerializerOptions.WriteIndented = true;
// });
// builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// await DbInitializer.SeedData(app);
app.UseStaticFiles();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
