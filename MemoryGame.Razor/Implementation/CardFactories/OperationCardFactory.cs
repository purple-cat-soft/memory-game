using MemoryGame.Models;

namespace MemoryGame.Razor.Implementation
{
  public class OperationsCardFactory : ICardFactory
  {
    private static readonly Random random = new Random(Environment.TickCount);

    public CardType CardType => CardType.Operations;

    public IEnumerable<Card> Generate(int numberOfCards)
    {
      for (var i = 0; i < numberOfCards / 2; i++)
      {
        var operation = GenerateOperation(i);

        yield return new Card(i.ToString(), i);
        yield return new Card(operation.ToString(), i);
      }
    }

    private Operation GenerateOperation(int result)
    {
      int left = random.Next(0, result);
      int right = result - left;

      var operation = new Operation
      {
        Left = left,
        Right = right,
        Operator = "+"
      };

      return operation;
    }

    public record Operation
    {
      public int Left { get; set; }
      public int Right { get; set; }
      public string Operator { get; set; }

      public override string ToString()
      {
        return $"{Left}{Operator}{Right}";
      }
    }
  }
}