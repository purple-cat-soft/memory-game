namespace MemoryGame.Models;

public class Card
{
  private static readonly Random _random = new Random(Environment.TickCount);

  public string Text { get; }

  public int Value { get; set; }

  public bool Turned { get; set; }

  public bool Matched { get; set; }

  public long UniqueId { get; set; }

  public Card(string text, int value)
  {
    Text = text;
    Value = value;

    Turned = false;
    Matched = false;
    UniqueId = _random.NextInt64();
  }
}