using MemoryGame.Models;

namespace MemoryGame.Razor.Implementation
{
  public interface ICardFactory
  {
    CardType CardType { get; }

    IEnumerable<Card> Generate(int numberOfCards);
  }
}