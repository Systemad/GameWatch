using Microsoft.EntityFrameworkCore;

namespace GameWatch.Persistence;

// https://learn.microsoft.com/en-us/aspnet/core/blazor/blazor-server-ef-core?view=aspnetcore-7.0
// Use this on Razor pages
public class UserContext : DbContext
{
    public DbSet<GameSchedule> GameSchedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(
                "Host=localhost;Port=5433;Database=gamewatch-db;Username=postgres;Password=Compaq2009"
            )
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}
