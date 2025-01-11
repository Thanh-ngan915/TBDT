using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shopping_Toturial.Reponsitory;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shopping_Toturial.Models;

var builder = WebApplication.CreateBuilder(args); 

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseMySql(
            connectionString, 
            ServerVersion.AutoDetect(connectionString)
    ));
    
    builder.Services.AddControllersWithViews();
    
    //session gio hang
    builder.Services.AddDistributedMemoryCache();
    
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(100);
        options.Cookie.IsEssential = true;
    });
    
    // identity
    builder.Services.AddIdentity<AppUserModel, IdentityRole>()
        .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(options =>
    {
        // Password settings.
        options.Password.RequireDigit = false; // yeu cau so
        options.Password.RequireLowercase = false; // yeu cau chu thuong
        options.Password.RequireNonAlphanumeric = false; // yeu cau ki tu dac biet
        options.Password.RequiredLength = 3; // do dai pass
        options.Password.RequireUppercase = false;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.Zero;
        options.Lockout.MaxFailedAccessAttempts = int.MaxValue;
        options.Lockout.AllowedForNewUsers = false;

        // User settings.
        options.User.RequireUniqueEmail = true;
    });

    
    var app = builder.Build(); 
    app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
    
    // build xong het nhung cong viec o tren thi su dung session
    app.UseSession();
    app.UseStaticFiles();

// Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
    }
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication(); // dang nhap

    app.UseAuthorization(); // quyen admin/user
    
    app.MapControllerRoute(
        name: "Areas",
        pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");
    
    // custom lai route cho mat phan slug=? cua category
    app.MapControllerRoute(
        name: "category",
        pattern: "/category/{slug?}",
        defaults: new { controller = "Category", action = "Index"});

    // custom lai route cho mat phan slug=? cua brand
    app.MapControllerRoute(
        name: "brand",
        pattern: "/brand/{slug?}",
        defaults: new { controller = "Brand", action = "Index"});

    
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
//Seeding data 
// var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DbContext>();
// // SeedData.SeedingData(context);
    app.Run();

