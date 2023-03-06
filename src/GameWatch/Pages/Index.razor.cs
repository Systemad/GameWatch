using System.Text.Json;
using GameWatch.Features.IGameDatabase;
using GameWatch.Features.IGameDatabase.Models;
using GameWatch.Persistence;
using GameWatch.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace GameWatch.Pages;

public partial class Index
{
    private DateTime CurrentDate = DateTime.UtcNow;
    private List<Game> Games;
    private bool loading = false;

    [Inject]
    private IGameDatabaseApi _gameDatabaseApi { get; set; }

    [Inject]
    private IDbContextFactory<GameContext> _context { get; set; }

    protected override async Task OnInitializedAsync()
    {
        //await GetReleasingGamesThisMonthAsync(_selectedMonth);
        await base.OnInitializedAsync();
    }

    private Task GetReleasingGamesThisMonthAsync(int month)
    {
        var thisYear = CurrentDate.Year;
        var unixPre = new DateTime(thisYear, month, 1).ConvertDateTimeToUnix();
        var unixPast = new DateTime(thisYear, month, 31).ConvertDateTimeToUnix();

        using var context = _context.CreateDbContext();
        Games = new List<Game>();
        var games = context.Games
            .Where(x => x.FirstReleaseDate > unixPre && x.FirstReleaseDate < unixPast)
            .ToList();
        Games = games;
        return Task.CompletedTask;
    }
}
