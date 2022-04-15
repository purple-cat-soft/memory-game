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

        /// <summary>
        /// The list of the cards
        /// </summary>
        public List<Card> CardList = new List<Card>();

        /// <summary>
        /// The number of cards allowed to reveal
        /// </summary>
        public int AllowToReveal => 6 - Level > 0 ? 6 - Level : 0;

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
            Level = 1;

            var cardPool = new List<string>();

            for (var i = 0; i < BoardSize * BoardSize / 2; i++ )
            {
                var card = _cards[_random.Next(42)];
                cardPool.Add(card);
                cardPool.Add(card);

            }
            Shuffle(cardPool);

            for (var i = 0; i < BoardSize; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    var card = cardPool.First();
                    cardPool.Remove(card);
                    CardList.Add(new Card(card));
                }
            }
        }

        public bool MatchIsFound()
        {
            return false;
        }

        private void Shuffle(List<string> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                string value = list[k];
                list[k] = list[n];
                list[n] = value;
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

        /// <summary>
        /// The card has already been matched
        /// </summary>
        public bool Matched { get; set; }

        public Card(string Text)
        {
            this.Text = Text;
            this.Revealed = false;
            this.Matched = false;
        }
    }
}
