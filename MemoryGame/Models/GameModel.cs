namespace MemoryGame.Models
{
    public class GameModel
    {
        /// <summary>
        /// Game level 1 - 4
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The size of the game board (unit being the number of cards)
        /// </summary>
        public int BoardSize { get; set; }

        public List<Card> CardList = new List<Card>();

        private string[] _cards = new string[42];

        private Random _random = new Random();

        public GameModel()
        {
            for (var i = 0; i < 16; i++)
            {
                _cards[i] = i.ToString();
            }
            for (var i = 0; i < 26; i++)
            {
                _cards[16 + i] = char.ConvertFromUtf32((int)('A') + i);
            }

            BoardSize = 4;

            for (var i = 0; i < BoardSize; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    CardList.Add(new Card(_cards[_random.Next(42)]));
                }
            }
        }
    }

    public class Card
    {
        /// <summary>
        /// Text on the card
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// If the card has been revealed
        /// </summary>
        public bool Revealed { get; set; }

        public Card(string Text)
        {
            this.Text = Text;
            this.Revealed = false;
        }
    }
}
