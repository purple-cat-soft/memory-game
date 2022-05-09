using Blazored.LocalStorage;
using MemoryGame.Shared;
using Timer = System.Timers.Timer;

namespace MemoryGame.Models
{
  public class GameModel
  {
    private readonly ILevelProvider mLevelProvider;
    private readonly Level mLevel;
    private static readonly Random _random = new Random(Environment.TickCount);

    public CardType CardType { get; private set; }

    public IList<Card> Cards { get; private set; }

    public int TimeCounter { get; set; }

    public Timer GameTimer { get; private set; }

    public bool Started { get; private set; }

    public bool Ended { get; private set; }

    public GameModel(ILevelProvider levelProvider, Level level, CardType cardType)
    {
      mLevelProvider = levelProvider;
      mLevel = level;
      Restart(level.Rows, level.Columns, cardType);
    }

    public void TurnBack()
    {
      foreach (Card card in Cards)
      {
        card.Turned = false;
      }
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

    public async Task Turn(Card card,CancellationToken cancellationToken)
    {
      Started = true;

      card.Turned = true;

      await Evaluate(cancellationToken);

      Ended = Cards.All(x => x.Turned);

      if (Ended)
      {
        mLevelProvider.LevelFinished(mLevel);
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
