using MemoryGame.Models;
using MemoryGame.Shared;
using Microsoft.AspNetCore.Components;

namespace MemoryGame.Pages;

public partial class Board
{
  private GameModel _model;

  [Inject]
  private ILevelProvider LevelProvider { get; set; }

  [Parameter]
  public int Level { get; set; }
  
  private int Rows { get; set; }
  private int Columns { get; set; }

  [Parameter]
  public string CardType { get; set; }

  protected override void OnInitialized()
  {
    Level selectedLevel = LevelProvider.GetLevel(Level);
    Rows = selectedLevel.Rows;
    Columns=selectedLevel.Columns;

    _model = new GameModel(selectedLevel, Models.CardType.Number);
  }
}
