using Blazored.LocalStorage;
using MemoryGame.Shared;
using Timer = System.Timers.Timer;

namespace MemoryGame.Models
{
  public class GameModel
  {
    private readonly Random _random = new Random(Environment.TickCount);

    public CardType CardType { get; private set; }

    public IList<Card> Cards { get; private set; }

    public int AllowToReveal => 2;

    public int TimeCounter { get; set; }

    public Timer DelayTimer { get; private set; }

    public Timer GameTimer { get; private set; }

    public bool Started { get; set; }

    public GameModel(Level level, CardType cardType)
    {
      DelayTimer = new Timer(1000);
      GameTimer = new Timer(1000);
      Cards = new List<Card>();
      Restart(level.Rows, level.Columns, cardType);
    }

    public void TurnBack()
    {
      foreach (Card card in Cards)
      {
        card.Revealed = false;
      }
    }

    public void Restart(int rows, int columns, CardType cardType)
    {
      CardType = cardType;
      DelayTimer = new Timer(1000);
      GameTimer = new Timer(1000);
      Cards = new List<Card>();
      Started = false;
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

    public async Task Match()
    {
      var groups = Cards.Where(x => x.Revealed && !x.Matched).GroupBy(x => x.Value).Where(x => x.Count() == 2);

      foreach (var group in groups)
      {
        foreach (var card in group)
        {
          card.Matched = true;
        }
      }

      await Task.Delay(2000);

      var upturnedCards = Cards.Where(x => !x.Matched && x.Revealed).ToArray();
      if (upturnedCards.Count() == 2)
      {
        foreach (Card card in upturnedCards)
        {
          card.Revealed = false;
        }
      }
    }
  }
}
