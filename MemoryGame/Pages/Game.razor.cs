using MemoryGame.Models;
using Microsoft.AspNetCore.Components;

namespace MemoryGame.Pages;

public partial class Game
{
  private GameModel gm;

  bool LimitReached = false;

  public int TimeCounter => gm.TimeCounter;

  private System.Timers.Timer _t;

  private int _level;

  private int _size;

  private CardType _cardType;

  [Parameter]
  public int Level
  {
    get
    {
      return gm.Level;
    }
    set
    {
      Restart(value, Size, CardType);
    }
  }

  [Parameter]
  public int Size
  {
    get
    {
      return gm.BoardSize;
    }
    set
    {
      Restart(Level, value, CardType);
    }
  }

  [Parameter]
  public string CardType
  {
    get
    {
      return gm.CardType.ToString();
    }
    set
    {
      Restart(Level, Size, value);
    }
  }

    protected override async Task OnInitializedAsync()
    {
        gm = new GameModel(1, 4, Models.CardType.Decimal);
        gm.NumOfImages = 0;
        while(await UrlValid($"images/flags/{gm.NumOfImages+1}.png"))
        {
            gm.NumOfImages++;
        }
        await gm.LoadAsync(localStore);
    }

  public async Task CardBackClickedAsync(int row, int col)
  {
    if (!gm.Started)
    {
      gm.Started = true;
      gm.GameTimer.Elapsed += this.GameTimerElapsed;
      gm.GameTimer.Start();
    }
    if (LimitReached)
    {
            gm.DelayTimer.Stop();
            gm.DelayTimer.Elapsed -= this.DelayTimerElapesd;
            gm.Cards.Where(c => !c.Matched).ToList().ForEach(c => c.Revealed = false);
            LimitReached = false;
    }
    gm.Cards.ElementAt(row * gm.BoardSize + col).Revealed = true;
    LimitReached = gm.Match();
    if (LimitReached)
    {
      gm.DelayTimer.Elapsed += this.DelayTimerElapesd;
      gm.DelayTimer.Start();
    }
    if (gm.GameOver)
    {
      gm.GameTimer.Elapsed -= this.GameTimerElapsed;
      gm.GameTimer.Stop();
      gm.GameTimer.Dispose();
      await gm.SaveAsync(localStore);
      await gm.LoadAsync(localStore);
    }
    StateHasChanged();
  }

  public void Restart(int level, int size, string cardType)
  {
    gm.DelayTimer.Stop();
    gm.DelayTimer.Elapsed -= this.DelayTimerElapesd;
    gm.GameTimer.Elapsed -= this.GameTimerElapsed;
    gm.GameTimer.Stop();
    gm.DelayTimer.Dispose();
    gm.GameTimer.Dispose();
    var delay = gm.Cards.Any(c => c.Revealed || c.Matched) ? 1000 : 1;

    gm.TurnBack();

    _level = level;
    _size = size;
    Enum.TryParse(cardType, out _cardType);
    _t = new System.Timers.Timer(delay);
    _t.Elapsed += f;
    _t.Start();
    StateHasChanged();
  }

  private void DelayTimerElapesd(object? sender, System.Timers.ElapsedEventArgs e)
  {
        gm.Cards.Where(c => !c.Matched).ToList().ForEach(c => c.Revealed = false);
        LimitReached = false;
        gm.DelayTimer.Stop();
        gm.DelayTimer.Elapsed -= this.DelayTimerElapesd;
        StateHasChanged();
    }

  private void GameTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
  {
    gm.TimeCounter += 1;
    StateHasChanged();
  }

  private void f(object? sender, System.Timers.ElapsedEventArgs e)
  {
    _t.Elapsed -= f;
    _t.Stop();
    _t.Dispose();
    gm.Restart(_level, _size, _cardType);
    StateHasChanged();
  }

    private async Task<bool> UrlValid(string url)
    {
        var result = await Http.GetAsync(url);
        return result.StatusCode == System.Net.HttpStatusCode.OK;
    }
}
