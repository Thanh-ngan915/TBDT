using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shopping_Toturial.Reponsitory;
using System;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args); 

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
));
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//Seeding data 
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DbContext>();
// SeedData.SeedingData(context);
app.Run();
