using MemoryGame.Models;

namespace MemoryGame.Razor.Implementation
{
  internal class CardFactory : ICardFactory
  {
    public IList<Card> GenerateCards(CardType cardType, int numberOfCards)
    {
      var cards = new List<Card>();
      if (cardType == CardType.Number)
      {
        for (var i = 0; i < numberOfCards / 2; i++)
        {
          cards.Add(new Card(i.ToString(), i));
          cards.Add(new Card(i.ToString(), i));
        }
      }

      return cards.OrderBy(x => x.UniqueId).ToArray(); ;
    }
  }
}
