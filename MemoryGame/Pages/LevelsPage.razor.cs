using System.Drawing;
using MemoryGame.Models;
using MemoryGame.Shared;
using Microsoft.AspNetCore.Components;

namespace MemoryGame.Pages;

public partial class LevelsPage
{
  [Inject]
  public  ILevelProvider LevelProvider { get; set; }

  private IList<Level> Levels { get; set; } = new List<Level>();

  protected override void OnInitialized()
  {
    Levels = LevelProvider.GetLevels(10).ToArray();
  }
}
