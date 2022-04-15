using System.Linq;
using System.Collections.Generic;
using System.Timers;
using Timer = System.Timers.Timer;

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

        public CardType CardType { get; set; }

        /// <summary>
        /// The list of the cards
        /// </summary>
        public List<Card> Cards;

        /// <summary>
        /// The number of cards allowed to reveal
        /// </summary>
        public int AllowToReveal => 6 - Level > 0 ? 6 - Level : 0;

        /// <summary>
        /// Game Timer
        /// </summary>
        public int TimeCounter { get; set; }

        public Timer DelayTimer { get; set; }

        public Timer GameTimer { get; set; }

        /// <summary>
        /// If the first click is made
        /// </summary>
        public bool Started { get; set; }

        public bool GameOver => Cards.All(c => c.Matched == true);

        private List<string> _cards = new List<string>();

        private Random _random = new Random();

        public GameModel(int level, int size, CardType cardType)
        {
            
            DelayTimer = new Timer(1000);
            GameTimer = new Timer(1000);
            Cards = new List<Card>();
            Restart(level, size, cardType);
        }

        public void TurnBack()
        {
            Cards.ForEach(c => c.Revealed = false);
        }

        public void Restart(int level, int size, CardType cardType)
        {
            BoardSize = size;
            Level = level;
            CardType = cardType;
            DelayTimer = new Timer(1000);
            GameTimer = new Timer(1000); 
            Cards = new List<Card>();
            TimeCounter = 0;

            _cards = new List<string>();
            if (cardType == CardType.Decimal)
            {
                for (var i = 0; i < 10; i++)
                {
                    _cards.Add(i.ToString());
                }
            }
            else if (cardType == CardType.Hexadecimal)
            {
                for (var i = 0; i < 16; i++)
                {
                    _cards.Add(i.ToString());
                }
            }
            else if (cardType == CardType.Alphabet)
            {
                for (var i = 0; i < 26; i++)
                {
                    _cards.Add(char.ConvertFromUtf32((int)('A') + i));
                }
            }

            var cardPool = new List<string>();

            for (var i = 0; i < BoardSize * BoardSize / 2; i++)
            {
                var card = _cards[_random.Next(_cards.Count)];
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
                    Cards.Add(new Card(card));
                }
            }
        }

        public bool Match()
        {
            bool isMatched = false;
            var revealedCards = Cards.Where(c => c.Revealed && !c.Matched);
            var matchedCards = Cards.Where(c1 => !c1.Matched && c1.Revealed && revealedCards.FirstOrDefault(c2 => !c2.Matched && c2.Revealed && c2 != c1 && c2.Text == c1.Text) != null);
            if(matchedCards.Any())
            {
                foreach (var card in matchedCards.ToList())
                {
                    card.Matched = true;
                    isMatched = true;
                }
            }

            return isMatched || revealedCards.Count() == AllowToReveal;
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


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Text">Card Text</param>
        public Card(string text, string image = "")
        {
            this.Text = text;
            this.Revealed = false;
            this.Matched = false;
        }
    }

    public enum CardType
    {
        Decimal,
        Hexadecimal,
        Alphabet,
        Images
    }
}
