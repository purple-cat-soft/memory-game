using MemoryGame.Models;

namespace MemoryGame.Razor.Implementation
{
  public interface ICardFactoryProvider
  {
    IList<Card> GenerateCards(CardType cardType, int numberOfCards);
  }
}