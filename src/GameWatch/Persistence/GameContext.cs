using GameWatch.Features.IGameDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GameWatch.Persistence;

public class GameContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    private static readonly IgnoringIdentityResolutionInterceptor IgnoringIdentityResolutionInterceptor =
        new();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            .AddInterceptors(IgnoringIdentityResolutionInterceptor)
            .UseNpgsql(
                "Host=localhost;Port=5433;Database=gamesdb;Username=postgres;Password=Compaq2009"
            )
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}
