using GameWatch.Features.User.Models;
using Microsoft.EntityFrameworkCore;

namespace GameWatch.Persistence;

// https://learn.microsoft.com/en-us/aspnet/core/blazor/blazor-server-ef-core?view=aspnetcore-7.0
// Use this on Razor pages
public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<GameReleaseSubscription> GameSchedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(
                "Host=localhost;Port=5433;Database=gamewatchdb;Username=postgres;Password=Compaq2009"
            )
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}
