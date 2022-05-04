using MemoryGame.Models;
using Microsoft.AspNetCore.Components;

namespace MemoryGame.Pages;

public partial class Board
{
  private GameModel _model;

  public int TimeCounter => _model.TimeCounter;

  [Parameter]
  public int Level
  {
    get => _model.Level;
    set => Restart(value, Size, CardType);
  }

  [Parameter]
  public int Size
  {
    get => _model.BoardSize;
    set => Restart(Level, value, CardType);
  }

  [Parameter]
  public string CardType
  {
    get => _model.CardType.ToString();
    set => Restart(Level, Size, value);
  }

  protected override async Task OnInitializedAsync()
  {
    _model = new GameModel(1, 4, Models.CardType.Number);
  }

  public void Restart(int level, int size, string cardType)
  {
    _model.TurnBack();
    StateHasChanged();
  }
}
