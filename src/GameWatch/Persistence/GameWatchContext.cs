using Microsoft.EntityFrameworkCore;

namespace GameWatch.Persistence;

// https://learn.microsoft.com/en-us/aspnet/core/blazor/blazor-server-ef-core?view=aspnetcore-7.0
// Use this on Razor pages
public class GameWatchContext : DbContext
{
    public GameWatchContext(DbContextOptions<GameWatchContext> options) : base(options) { }

    public DbSet<GameSchedule> GameSchedules { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    //    optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_pw");
}
