namespace GameWatch.Pages;

public partial class Index
{
    public DateTime CurrentDate;

    protected override async Task OnInitializedAsync()
    {
        CurrentDate = new DateTime();
        await base.OnInitializedAsync();
    }
}
