using Application.Baskets;
using Application.Catalogs.CatalogItems.CatalogItemSeviceForWebSite;
using Application.Catalogs.CatalogItems.GetCatalogIItemPLP;
using Application.Catalogs.CatalogItems.GetCatalogItemPDP;
using Application.Catalogs.CatalogItems.UriComposer;
using Application.Catalogs.GetMenuItem;
using Application.Contexts.Interfaces;
using Application.Discounts;
using Application.Interfaces.Contexts;
using Application.Orders;
using Application.Payments;
using Application.Services.HomePage;
using Application.Services.Orders.CustomerOrdersServices;
using Application.Users;
using Application.Visitors.SaveVisitorInfo;
using Application.Visitors.VisitorOnline;
using Infrastructure.IdentityConfigs;
using Infrastructure.MappingProfile;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Contexts.MongoContext;
using WebSite.EndPoint.Hubs;
using WebSite.EndPoint.Middlewares;
using WebSite.EndPoint.Models.Utility.Filters;
using WebSite.EndPoint.Utility.Middlewares;
using Microsoft.Extensions.Caching.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region ConnectionString
string connectionString = builder.Configuration["ConnectionString:SqlServer"];
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
#endregion

#region IdentityConfig
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    option.LoginPath = "/account/login";
    option.SlidingExpiration = true;
    option.AccessDeniedPath = "/Account/AccessDenied";

});
#endregion

builder.Services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));
builder.Services.AddTransient<ISaveVisitorInfoService, SaveVisitorInfoService>();
builder.Services.AddScoped<SaveVisitorFilter>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IVisitorOnlineService, VisitorOnlineService>();
builder.Services.AddTransient<IGetMenuItemService, GetMenuItemService>();
builder.Services.AddTransient<IGetCatalogIItemPLPService, GetCatalogIItemPLPService>();
builder.Services.AddTransient<IUriComposerService, UriComposerService>();
builder.Services.AddTransient<IGetCatalogItemPDPService, GetCatalogItemPDPService>();
builder.Services.AddTransient<IBasketService, BasketService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IUserAddressService, UserAddressService>();
builder.Services.AddTransient<ICatalogItemSeviceForWebSite, CatalogItemSeviceForWebSite>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IDiscountHistoryService, DiscountHistoryService>();
builder.Services.AddTransient<IDiscountService, DiscountService>();
builder.Services.AddTransient<IIdentityDatabaseContext, IdentityDataBaseContext>();
builder.Services.AddTransient<IHomePageService, HomePageService>();
builder.Services.AddTransient<ICustomerOrdersService, CustomerOrdersService>();
builder.Services.AddSignalR();

builder.Services.AddDistributedSqlServerCache(option =>
{
    option.ConnectionString = connectionString;
    option.SchemaName = "dbo";
    option.TableName = "CacheData";
});


//mapper
builder.Services.AddAutoMapper(typeof(CatalogMappingProfile));
builder.Services.AddAutoMapper(typeof(CatalogMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCustomExceptionHandler();
app.UseSaveVisitorIdInCookie();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    //endpoints.MapControllerRoute(
    //name: "ProductDetails",
    //pattern: "product/{id}/{slug}",
    //defaults: new { controller = "Product", action = "Details" });
});
app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<OnlineVisitorHub>("/chathub");




app.Run();
