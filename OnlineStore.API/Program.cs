using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.API.Data;
using OnlineStore.API.Middleware;
using OnlineStore.API.Repositories;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<GlobalExceptionMiddleware>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ITokenRepository, TokenRepository>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<StoreAuthDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<StoreAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreAuthConnection")));

var app = builder.Build();

// First Middleware to handle global exceptions
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Second Inline Request logging middleware
app.Use(async (HttpContext context, Func<Task> next) =>
{
    // 1. Start measuring the request duration
    var stopwatch = Stopwatch.StartNew();

    // 2. Get the HTTP method and request path
    var method = context.Request.Method;
    var path = context.Request.Path;

    try
    {
        // 3. Pass the request to the next middleware
        await next();
    }
    finally
    {
        // 4. Stop the timer after request processing
        stopwatch.Stop();

        // 5. Get the final response status code
        var statusCode = context.Response.StatusCode;

        // 6. Log the request details
        Console.WriteLine(
            $"{method} {path} -> {statusCode} -> {stopwatch.ElapsedMilliseconds} ms");
    }
});



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.Run();
