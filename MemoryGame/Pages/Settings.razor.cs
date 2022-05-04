using MemoryGame.Models;
using Microsoft.AspNetCore.Components;

namespace MemoryGame.Pages;

public partial class Settings
{
  private GameModel _model;
  private bool _limitReached;
  private System.Timers.Timer _timer;
  private int _level;
  private int _size;
  private CardType _cardType;

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
  
    await _model.LoadAsync(localStore);
  }

  public void Restart(int level, int size, string cardType)
  {
    _model.DelayTimer.Stop();
    _model.DelayTimer.Elapsed -= this.DelayTimerElapsed;
    _model.GameTimer.Elapsed -= this.GameTimerElapsed;
    _model.GameTimer.Stop();
    _model.DelayTimer.Dispose();
    _model.GameTimer.Dispose();
    var delay = _model.Cards.Any(c => c.Revealed || c.Matched) ? 1000 : 1;

    _model.TurnBack();

    _level = level;
    _size = size;
    Enum.TryParse(cardType, out _cardType);
    _timer = new System.Timers.Timer(delay);
    _timer.Elapsed += RestartDelay;
    _timer.Start();
    StateHasChanged();
  }

  private void DelayTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
  {
    _model.Cards.Where(c => !c.Matched).ToList().ForEach(c => c.Revealed = false);
    _limitReached = false;
    _model.DelayTimer.Stop();
    _model.DelayTimer.Elapsed -= this.DelayTimerElapsed;
    StateHasChanged();
  }

  private void GameTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
  {
    _model.TimeCounter += 1;
    StateHasChanged();
  }

  private void RestartDelay(object sender, System.Timers.ElapsedEventArgs e)
  {
    _timer.Elapsed -= RestartDelay;
    _timer.Stop();
    _timer.Dispose();
    _model.Restart(_level, _size, _cardType);
    StateHasChanged();
  }

  
}
