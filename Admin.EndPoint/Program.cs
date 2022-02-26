using Admin.EndPoint.MappingProfile;
using Application.Catalogs.CatalogTypes.CrudService;
using Application.Contexts.Interfaces;
using Application.Interfaces.Contexts;
using Application.Visitors.GetVisitorReports;
using Infrastructure.MappingProfile;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Contexts.MongoContext;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IGetVisitorReportService, GetVisitorReportService>();
builder.Services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));

#region ConnectionString
string connectionString = builder.Configuration["ConnectionString:SqlServer"];
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();
#endregion

builder.Services.AddTransient<ICatalogTypeService, CatalogTypeService>();


//mapper
builder.Services.AddAutoMapper(typeof(CatalogMappingProfile));
builder.Services.AddAutoMapper(typeof(CatalogVMMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
