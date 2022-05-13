using System.Diagnostics;
using MemoryGame.Shared;
using Timer = System.Timers.Timer;

namespace MemoryGame.Models
{
  public class GameModel
  {
    private static readonly Random _random = new Random(Environment.TickCount);

    private readonly ILevelProvider mLevelProvider;
    private readonly IApplicationService mApplicationService;
    private readonly Level mLevel;

    private Timer mGameTimer;
    private Stopwatch mStopwatch;
    private int mGameCount;
    
    public GameModel(ILevelProvider levelProvider, IApplicationService applicationService, Level level, CardType cardType)
    {
      mLevelProvider = levelProvider;
      mApplicationService = applicationService;
      mLevel = level;
      Restart(level.Rows, level.Columns, cardType);
    }

    public CardType CardType { get; private set; }

    public IList<Card> Cards { get; private set; }

    public int TimeCounter { get; set; }

    public bool IsStarted { get; private set; }

    public bool Ended { get; private set; }

    public void Restart(int rows, int columns, CardType cardType)
    {
      CardType = cardType;
      mGameTimer = new Timer(1000);
      Cards = new List<Card>();
      IsStarted = false;
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
      if (IsStarted == false)
      {
        Start();
      }

      mGameCount++;
      SetTitle();

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
      Cancel();
    }

    public void Cancel()
    {
      mStopwatch?.Stop();
      mGameTimer?.Dispose();
      mGameTimer = null;
      mStopwatch = null;
    }

    private void Start()
    {
      mGameTimer = new Timer(500);
      mGameTimer.Start();
      mGameTimer.Elapsed += (_, _) => SetTitle();

      mStopwatch = new Stopwatch();
      mStopwatch.Start();

      IsStarted = true;

      SetTitle();
    }

    private void SetTitle()
    {
      if (!Ended)
      {
        mApplicationService.SetTitle($"{mStopwatch.Elapsed.TotalMinutes:00}:{mStopwatch.Elapsed.Seconds:00} \\ {mGameCount}");
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