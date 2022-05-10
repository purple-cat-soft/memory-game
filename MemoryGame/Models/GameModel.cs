using System.Diagnostics;
using Blazored.LocalStorage;
using MemoryGame.Shared;
using Timer = System.Timers.Timer;

namespace MemoryGame.Models
{
  public class GameModel
  {
    private readonly ILevelProvider mLevelProvider;
    private readonly IApplicationService mApplicationService;
    private readonly Level mLevel;
    private static readonly Random _random = new Random(Environment.TickCount);

    public CardType CardType { get; private set; }

    public IList<Card> Cards { get; private set; }

    public int TimeCounter { get; set; }

    public Timer GameTimer { get; private set; }
    public Stopwatch Stopwatch { get; private set; }

    public bool Started { get; private set; }

    public bool Ended { get; private set; }

    public GameModel(ILevelProvider levelProvider, IApplicationService applicationService, Level level, CardType cardType)
    {
      mLevelProvider = levelProvider;
      mApplicationService = applicationService;
      mLevel = level;
      Restart(level.Rows, level.Columns, cardType);
    }

    public void Restart(int rows, int columns, CardType cardType)
    {
      CardType = cardType;
      GameTimer = new Timer(1000);
      Cards = new List<Card>();
      Started = false;
      Ended = false;
      TimeCounter = 0;

      Cards = FillCards(rows * columns, cardType).ToArray();
    }

    private IList<Card> FillCards(int numberOfCards, CardType cardType)
    {
      var cards = new List<Card>();

      if (cardType == CardType.Number)
      {
        for (var i = 0; i < numberOfCards / 2; i++)
        {
          cards.Add(new Card(_random.NextInt64(), i.ToString(), i));
          cards.Add(new Card(_random.NextInt64(), i.ToString(), i));
        }
      }

      return cards.OrderBy(x => x.UniqueId).ToArray();
    }

    public async Task Turn(Card card, CancellationToken cancellationToken)
    {
      if (Started == false)
      {
        Start();
      }

      card.Turned = true;

      Ended = Cards.All(x => x.Turned);

      if (Ended)
      {
        End();
      }

      await Evaluate(cancellationToken);
    }

    public void End()
    {
      mLevelProvider?.LevelFinished(mLevel);

      Stopwatch?.Stop();
      GameTimer?.Dispose();
      GameTimer = null;
      Stopwatch = null;
    }

    private void Start()
    {
      GameTimer = new Timer(250);
      GameTimer.Start();
      GameTimer.Elapsed += (_, _) => SetTitle();

      Stopwatch = new Stopwatch();
      Stopwatch.Start();

      Started = true;

      SetTitle();
    }

    private void SetTitle()
    {
      if (!Ended)
      {
        mApplicationService.SetTitle($"{Stopwatch.Elapsed.TotalMinutes:00}:{Stopwatch.Elapsed.Seconds:00}");
      }
    }

    private async Task Evaluate(CancellationToken cancellationToken)
    {
      var groups = Cards.Where(x => x.Turned && !x.Matched).GroupBy(x => x.Value).Where(x => x.Count() == 2);

      foreach (var group in groups)
      {
        foreach (var card in group)
        {
          card.Matched = true;
        }
      }

      await Task.Delay(2000, cancellationToken);

      if (!cancellationToken.IsCancellationRequested)
      {
        TryClear();
      }
    }

    public void TryClear()
    {
      var upturnedCards = Cards.Where(x => !x.Matched && x.Turned).ToArray();
      if (upturnedCards.Count() > 1)
      {
        foreach (Card card in upturnedCards)
        {
          card.Turned = false;
        }
      }
    }
  }
}
