using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using GameWatch.Data;
using GameWatch.Features.Auth;
using GameWatch.Features.IGameDatabase;
using GameWatch.Persistence;
using MudBlazor.Services;
using Quartz;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration().MinimumLevel
    .Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddMemoryCache();

// Builds options with Twitch keys, which will be used from IOption<TwitchOptions>
builder.Services
    .AddOptions<TwitchOptions>()
    .Bind(builder.Configuration.GetSection(TwitchOptions.Twitch));

// Add Azure AD B2C services to the container
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

// User context is used for users information only
builder.Services.AddDbContextFactory<UserContext>();

// Game context is used for storing all game information database
builder.Services.AddDbContextFactory<GameContext>();

// Twitch token service, which generates and/or refreshes Twitch access token
builder.Services.AddSingleton<ITwitchAccessTokenService, TwitchAccessTokenService>();

// IGDB Database service
builder.Services.AddSingleton<IGameDatabaseApi, GameDatabaseApi>();

builder.Services.AddQuartz(
    q =>
    {
        q.UseMicrosoftDependencyInjectionScopedJobFactory();
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

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

if (builder.Environment.IsDevelopment())
{
    //var context = scope.ServiceProvider.GetRequiredService<DbContext<GameWatchContext>>();
    await using var context = new GameContext();
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();
}

/*
var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
var scheduler = await schedulerFactory.GetScheduler();

// This job triggers every 24 hours for 2 minutes to fetch all games from IGDB and store into a database
var job = JobBuilder.Create<FetchGamesJob>().WithIdentity("FetchGamesJob", "igdb").Build();

var trigger = TriggerBuilder
    .Create()
    .WithIdentity("FetchGamesJob-trigger", "igdb")
    .StartNow()
    .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
    .Build();

await scheduler.ScheduleJob(job, trigger);
*/

app.Run();
