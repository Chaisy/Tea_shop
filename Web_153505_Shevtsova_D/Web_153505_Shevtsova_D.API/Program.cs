using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.API.Data;
using Web_153505_Shevtsova_D.API.Services.CategoryService;
using Web_153505_Shevtsova_D.API.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WriteIndented = true;
});
 
var app = builder.Build();

// await DbInitializer.SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();
app.UseRouting();

app.Run();