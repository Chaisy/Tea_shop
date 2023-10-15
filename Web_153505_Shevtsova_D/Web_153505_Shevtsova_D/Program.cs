using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.Data;
using Web_153505_Shevtsova_D.Services.ProductService;
using Web_153505_Shevtsova_D.Services.TeaBasesService;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

 builder.Services.AddScoped(typeof(ICategoryService),typeof(MemoryCategoryService));
 builder.Services.AddScoped(typeof(IProductService), typeof(MemoryProductService));

var uriData = builder.Configuration["UriData:ApiUri"];

builder.Services
    .AddHttpClient<IProductService, ApiProductService>(opt =>
        opt.BaseAddress = new Uri(uriData!));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();





// Add services to the container.































