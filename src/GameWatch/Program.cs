using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using GameWatch.Data;
using GameWatch.Features.Auth;
using GameWatch.Features.IGameDatabase;
using GameWatch.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Quartz;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().MinimumLevel
    .Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services
    .AddOptions<TwitchOptions>()
    .Bind(builder.Configuration.GetSection(TwitchOptions.Twitch));

// Add services to the container.
builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(
    options =>
    {
        // By default, all incoming requests will be authorized according to the default policy
        options.FallbackPolicy = options.DefaultPolicy;
    }
);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMudServices();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContextFactory<GameWatchDbContext>(
        options =>
            options
                .UseNpgsql(builder.Configuration.GetConnectionString("GameWatchDbContext"))
                .EnableSensitiveDataLogging()
    );
}
else
{
    builder.Services.AddDbContext<GameWatchDbContext>(
        options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("GameWatchDbContext"))
    );
}

builder.Services.AddQuartz();
builder.Services.AddQuartzServer(
    options =>
    {
        // when shutting down we want jobs to complete gracefully
        options.WaitForJobsToComplete = true;
    }
);

builder.Services.AddSingleton<ITwitchAccessTokenService, TwitchAccessTokenService>();
builder.Services.AddSingleton<IGameDatabaseApi, GameDatabaseApi>();

/*
builder.Services.AddHttpClient(
    "GameDatabase",
    client =>
    {
        client.BaseAddress = new Uri("https://api.igdb.com/v4/");
    }
);
*/

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

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

if (builder.Environment.IsDevelopment())
{
    //await using var scope = app.Services
    //    .GetRequiredService<IServiceScopeFactory>()
    //    .CreateAsyncScope();
    //var context = scope.ServiceProvider.GetRequiredService<DbContext<GameWatchContext>>();
    await using var context = new GameWatchDbContext();
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();
}

app.Run();


// TOOD: https://learn.microsoft.com/en-us/aspnet/core/blazor/security/server/?view=aspnetcore-7.0&tabs=visual-studio#notification-about-authentication-state-changes
