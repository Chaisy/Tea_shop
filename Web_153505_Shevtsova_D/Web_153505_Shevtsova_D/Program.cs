using Web_153505_Shevtsova_D.Services;
using Web_153505_Shevtsova_D.Services.ProductService;
using Web_153505_Shevtsova_D.Services.TeaBasesService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
builder.Services.AddScoped<IProductService, ApiProductService>();

var uriData = builder.Configuration["UriData:ApiUri"];
builder.Services
    .AddHttpClient<IProductService, ApiProductService>(opt =>
        opt.BaseAddress = new Uri(uriData!));
builder.Services
    .AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
        opt.BaseAddress = new Uri(uriData!));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
