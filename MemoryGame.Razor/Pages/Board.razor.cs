using MemoryGame.Models;
using MemoryGame.Razor.Implementation;
using MemoryGame.Shared;
using Microsoft.AspNetCore.Components;

namespace MemoryGame.Razor.Pages;

public partial class Board : IDisposable
{
  private GameModel mModel;
  private CancellationTokenSource mCancellationTokenSource;

  [Inject]
  private ILevelProvider LevelProvider { get; set; }

  [Inject]
  public IApplicationService ApplicationService { get; set; }

  [Inject]
  public ICardFactoryProvider CardFactory { get; set; }

  [Parameter]
  public int Level { get; set; }

  [Parameter]
  public string CardType { get; set; }

  protected override async Task OnInitializedAsync()
  {
    ApplicationService.SetTitle("Click to start the game");

    Level selectedLevel = await LevelProvider.GetLevel(Level == 0 ? 1 : Level);
    Rows = selectedLevel.Rows;
    Columns = selectedLevel.Columns;

    mModel = new GameModel(LevelProvider, ApplicationService, CardFactory, selectedLevel, Models.CardType.Numbers);

    IsInitialized = true;
  }

  private int Rows { get; set; }
  private int Columns { get; set; }
  private bool IsInitialized { get; set; }

  private async Task OnClick(Card card)
  {
    mCancellationTokenSource?.Cancel();

    mModel.TryClear();

    mCancellationTokenSource = new CancellationTokenSource();

    await mModel.Turn(card, mCancellationTokenSource.Token);
  }

  private string GetCssClass(Card card)
  {
    if (card.Matched)
    {
      return "match";
    }

    //if (card.Revealed)
    //{
    //  return "upturned";
    //}

    return string.Empty;
  }

  private string GetContent(Card card)
  {
    return card.Turned ? card.Text : string.Empty;
  }

  private string GetCellSize()
  {
    var rows = (int)Math.Round(90m / Rows, MidpointRounding.ToZero);
    var columns = (int)Math.Round(90m / Rows, MidpointRounding.ToZero);
    int min = Math.Min(rows, columns);

    return $"min(calc(100vh / {Rows} - 45px / {Rows} - 5px) , calc(100vw / {Columns} - 10px / {Columns} - 5px))";
  }

  public void Dispose()
  {
    mModel.Cancel();
  }
}
