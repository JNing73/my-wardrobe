using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyWardrobe.Data;
using MyWardrobe.Models.Initialisers;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyWardrobeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyWardrobeContext") ?? throw new InvalidOperationException("Connection string 'MyWardrobeContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Add default data when database is empty
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ClothingItems}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
