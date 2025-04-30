using CryptoMonitor.Data;
using CryptoMonitor.Models;
using CryptoMonitor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Добавляем сервисы Blazorise
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

builder.Services.AddDbContext<CryptoMonitorDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient("coinGecko", c =>
{
    c.BaseAddress = new Uri("https://api.coingecko.com/api/v3/");
    c.DefaultRequestHeaders.UserAgent.ParseAdd("CryptoMonitorApp/1.0");
    c.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHostedService<CryptoDataService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CryptoMonitorDbContext>();

    // Seed 
    if (!db.Roles.Any())
    {
        db.Roles.AddRange(
            new Role { RoleName = "Admin" },
            new Role { RoleName = "User" },
            new Role { RoleName = "Guest" }
        );
    }

    // Seed  CoinGecko
    if (!db.ApiSources.Any(a => a.Name == "CoinGecko"))
    {
        db.ApiSources.Add(new ApiSource
        {
            Name = "CoinGecko",
            BaseUrl = "https://api.coingecko.com/api/v3",
            Description = "Public crypto data API",
            IsActive = true
        });
    }

    db.SaveChanges();
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
