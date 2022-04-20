namespace MemoryGame.Models
{
  public class Card
  {
    public string Text { get; }

    public int Index { get; set; }

    public bool Revealed { get; set; }

    public bool Matched { get; set; }

    public Card(string text, int index, string image = "")
    {
      this.Text = text;
      this.Index = index;
      this.Revealed = false;
      this.Matched = false;
    }
  }
}
