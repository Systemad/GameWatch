using GameWatch.Features.IGameDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GameWatch.Persistence;

public class GameContext : DbContext
{
    public DbSet<Game> Games { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            .UseNpgsql(
                "Host=localhost;Port=5433;Database=gamesdb;Username=postgres;Password=Compaq2009"
            )
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}
