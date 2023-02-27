using GameWatch.Common.Game;
using GameWatch.Features.IGameDatabase;
using Microsoft.AspNetCore.Components;

namespace GameWatch.Pages;

public partial class Index
{
    private DateTime _selectedDate = DateTime.UtcNow;
    private List<Game> Games = new();

    [Inject]
    private IGameDatabaseApi _gameDatabaseApi { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetGamesForMonth(_selectedDate.Year, _selectedDate.Month);
        await base.OnInitializedAsync();
    }

    // TODO: For 2 MudSelect
    private async Task GetGamesForMonth(int year, int month)
    {
        var games = await _gameDatabaseApi.GetGamesByMonthAsync(year, month);
        Games = games.ToList();
    }
}
