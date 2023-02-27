using GameWatch.Features.IGameDatabase;
using Microsoft.EntityFrameworkCore;

namespace GameWatch.Persistence;

// https://learn.microsoft.com/en-us/aspnet/core/blazor/blazor-server-ef-core?view=aspnetcore-7.0
// Use this on Razor pages
public class GameWatchDbContext : DbContext
{
    //public GameWatchContext(DbContextOptions<GameWatchContext> options) : base(options) { }

    public DbSet<GameSchedule> GameSchedules { get; set; }
    public DbSet<Game> Games { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5433;Database=gamewatch-db;Username=postgres;Password=Compaq2009"
        );
}
