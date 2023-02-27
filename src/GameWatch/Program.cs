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
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddMemoryCache();
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
builder.Services.AddSingleton<IGameDatabaseApi, GameDatabaseApi>();

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

builder.Services.AddSingleton<ITwitchAccessTokenService, TwitchAccessTokenService>();
builder.Services.AddSingleton<IGameDatabaseApi, GameDatabaseApi>();

builder.Services.AddQuartz(
    q =>
    {
        q.UseMicrosoftDependencyInjectionScopedJobFactory();
        /*
        var jobKey = new JobKey("FetchGamesJob");
        q.AddJob<FetchGamesJob>(opts => opts.WithIdentity(jobKey));
        q.AddTrigger(
            opts =>
                opts.ForJob(jobKey)
                    .WithIdentity("FetchGamesJob-trigger")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInHours(6).RepeatForever())
        );
        */
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
    //await using var scope = app.Services
    //    .GetRequiredService<IServiceScopeFactory>()
    //    .CreateAsyncScope();
    //var context = scope.ServiceProvider.GetRequiredService<DbContext<GameWatchContext>>();
    await using var context = new GameWatchDbContext();
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();
}

var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
var scheduler = await schedulerFactory.GetScheduler();

var job = JobBuilder.Create<FetchGamesJob>().WithIdentity("FetchGamesJob", "igdb").Build();

// Trigger the job to run now, and then every 40 seconds
var trigger = TriggerBuilder
    .Create()
    .WithIdentity("FetchGamesJob-trigger", "igdb")
    .StartNow()
    .WithSimpleSchedule(x => x.WithIntervalInHours(6).RepeatForever())
    .Build();

await scheduler.ScheduleJob(job, trigger);

app.Run();
