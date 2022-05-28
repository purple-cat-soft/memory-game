using MemoryGame.Models;

namespace MemoryGame.Razor.Implementation
{
  internal class CardFactoryProvider : ICardFactoryProvider
  {
    private readonly IList<ICardFactory> mCardFactories;

    public CardFactoryProvider(IEnumerable<ICardFactory> cardFactories)
    {
      mCardFactories = cardFactories.ToArray();
    }

    public IList<Card> GenerateCards(CardType cardType, int numberOfCards)
    {
      var cardFactory = mCardFactories.Single(x => x.CardType == cardType);

      var cards = cardFactory.Generate(numberOfCards).ToArray();


      return cards.OrderBy(x => x.UniqueId).ToArray(); ;
    }
  }
}