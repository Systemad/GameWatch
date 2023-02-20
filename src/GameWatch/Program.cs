using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using GameWatch.Data;
using GameWatch.Features.Auth;
using GameWatch.Features.IGameDatabase;
using GameWatch.Persistence;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Quartz;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().MinimumLevel
    .Override("Microsoft.AspNetCore", LogEventLevel.Warning)
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
    builder.Services.AddDbContext<GameWatchContext>(
        options =>
            options
                .UseNpgsql(builder.Configuration.GetConnectionString("BloggingContext"))
                .EnableSensitiveDataLogging()
    );
}
else
{
    builder.Services.AddDbContext<GameWatchContext>(
        options => options.UseNpgsql(builder.Configuration.GetConnectionString("BloggingContext"))
    );
}

builder.Services.AddQuartz(
    q =>
    {
        q.UseMicrosoftDependencyInjectionJobFactory();
        // Split up to seperate class
        var fetchTwitchTokenJobKey = new JobKey("FetchTwitchTokenJobKey");
        q.AddJob<FetchTwitchTokenJob>(ops => ops.WithIdentity(fetchTwitchTokenJobKey));
        q.AddTrigger(
            opts =>
            {
                opts.ForJob(fetchTwitchTokenJobKey)
                    .WithIdentity("FetchTwitchTokenJobKey-trigger")
                    .WithCronSchedule("0 2 * * 0"); // every sunday at 2AM
            }
        );
    }
);
builder.Services.AddQuartzServer(
    options =>
    {
        // when shutting down we want jobs to complete gracefully
        options.WaitForJobsToComplete = true;
    }
);

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

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
