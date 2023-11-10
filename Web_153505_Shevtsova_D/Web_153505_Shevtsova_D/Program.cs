using Microsoft.IdentityModel.Logging;
using Web_153505_Shevtsova_D.Services;
using Web_153505_Shevtsova_D.Services.ProductService;
using Web_153505_Shevtsova_D.Services.TeaBasesService;
using Web_153505_Shevtsova_D.Domain.Models;
using Web_153505_Shevtsova_D.Models;

var builder = WebApplication.CreateBuilder(args);
IdentityModelEventSource.ShowPII = true;
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
builder.Services.AddScoped<IProductService, ApiProductService>();

var uriData = builder.Configuration["UriData:ApiUri"];
builder.Services
    .AddHttpClient<IProductService, ApiProductService>(opt =>
        opt.BaseAddress = new Uri(uriData!));
builder.Services
    .AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
        opt.BaseAddress = new Uri(uriData!));
builder.Services.AddRazorPages();

builder.Services.AddScoped(typeof(Cart), sp => SessionCart.GetCart(sp));

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = "cookie";
        opt.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
        options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
        options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];
        // Ïîëó÷èòü Claims ïîëüçîâàòåëÿ
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.SaveTokens = true;
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

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


app.UseStaticFiles();

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

app.MapRazorPages().RequireAuthorization();

app.Run();
