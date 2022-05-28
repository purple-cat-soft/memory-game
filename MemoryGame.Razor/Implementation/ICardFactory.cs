using MemoryGame.Models;

namespace MemoryGame.Razor.Implementation
{
  public interface ICardFactory
  {
    IList<Card> GenerateCards(CardType cardType, int numberOfCards);
  }
}