namespace MemoryGame.Models
{
    public class Card
    {
        public string Text { get; }

        public bool Revealed { get; set; }

        public bool Matched { get; set; }

        public Card(string text, string image = "")
        {
            this.Text = text;
            this.Revealed = false;
            this.Matched = false;
        }
    }
}
