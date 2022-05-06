using MemoryGame.Shared;
using Microsoft.AspNetCore.Components;

namespace MemoryGame.Pages;

public partial class LevelsPage
{
  [Inject]
  public  ILevelProvider LevelProvider { get; set; }

  [Inject]
  private NavigationManager NavigationManager { get; set; }

  private IList<Level> Levels { get; set; } = new List<Level>();

  protected override void OnInitialized()
  {
    Levels = LevelProvider.Levels;
  }

  private void OnLevelSelected(int level)
  {
    NavigationManager.NavigateTo($"/index/{level}");
  }
}
