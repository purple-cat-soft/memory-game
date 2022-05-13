namespace MemoryGame.Models
{
  public class Card
  {
    public string Text { get; }

    public int Value { get; set; }

    public bool Turned { get; set; }

    public bool Matched { get; set; }

    public long UniqueId { get; set; }

    public Card(long uniqueId, string text, int value, string image = "")
    {
      UniqueId = uniqueId;
      Text = text;
      Value = value;
      Turned = false;
      Matched = false;
    }
  }
}
