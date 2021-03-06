using MemoryGame.Shared;
using Microsoft.AspNetCore.Components;

namespace MemoryGame.Razor.Pages;

public partial class LevelsPage
{
  [Inject]
  public  ILevelProvider LevelProvider { get; set; }

  [Inject]
  public IApplicationService ApplicationService { get; set; }

  [Inject]
  private NavigationManager NavigationManager { get; set; }

  private IList<Level> Levels { get; set; } = new List<Level>();

  protected override async Task OnInitializedAsync()
  {
    ApplicationService.SetTitle("Select the level");
    Levels =await LevelProvider.GetLevels();
  }

  private void OnLevelSelected(int level)
  {
    NavigationManager.NavigateTo($"./index/{level}");
  }
}
