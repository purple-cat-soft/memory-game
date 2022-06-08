using MemoryGame.Models;

namespace MemoryGame.Razor.Implementation
{
  public class NumbersCardFactory : ICardFactory
  {
    public CardType CardType => CardType.Numbers;

    public IEnumerable<Card> Generate(int numberOfCards)
    {
      for (var i = 0; i < numberOfCards / 2; i++)
      {
        yield return new Card(i.ToString(), i);
        yield return new Card(i.ToString(), i);
      }
    }
  }
}