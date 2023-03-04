using GameWatch.Features.IGameDatabase;
using GameWatch.Features.IGameDatabase.Models;
using GameWatch.Persistence;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace GameWatch.Pages;

public partial class Index
{
    private DateTime _selectedDate = DateTime.UtcNow;
    private List<Game> Games;
    private bool loading = false;

    [Inject]
    private IGameDatabaseApi _gameDatabaseApi { get; set; }

    [Inject]
    private IDbContextFactory<UserContext> _context { get; set; }

    protected override async Task OnInitializedAsync()
    {
        //await GetGamesForMonth(_selectedDate.Year, _selectedDate.Month);
        await base.OnInitializedAsync();
    }

    private async Task GetGamesForMonth(int year, int month)
    {
        using var context = _context.CreateDbContext();
        Games = new List<Game>();
        var games = await _gameDatabaseApi.GetGamesByMonthAsync(year, month);
        var ids = games.Select(x => x.Id);
        //Games = context.Games.Where(x => ids.Contains(x.Id)).ToList();
    }
}
