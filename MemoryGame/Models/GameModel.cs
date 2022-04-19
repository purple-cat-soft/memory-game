using System.Linq;
using System.Collections.Generic;
using System.Timers;
using Blazored.LocalStorage;
using Timer = System.Timers.Timer;

namespace MemoryGame.Models
{
    public class GameModel
    {
        private Random _random = new Random(Environment.TickCount);
        private List<string> _cards = new List<string>();

        public int Level { get; private set; }

        public int BoardSize { get; private set; }

        public CardType CardType { get; private set; }

        public List<Card> Cards { get; private set; }

        public int AllowToReveal => 6 - Level > 0 ? 6 - Level : 0;

        public int TimeCounter { get; set; }

        public Timer DelayTimer { get; private set; }

        public Timer GameTimer { get; private set; }

        public bool Started { get; set; }

        public bool GameOver => Cards.All(c => c.Matched == true);

        public Dictionary<(int, int, string), int> Records = new Dictionary<(int, int, string), int>();

        public int NumOfImages { get; set; }

        public GameModel(int level, int size, CardType cardType)
        {
            DelayTimer = new Timer(1000);
            GameTimer = new Timer(1000);
            Cards = new List<Card>();
            InitializeRecords();
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
            Started = false;
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
            else if (cardType == CardType.Images)
            {
                for (var i = 1; i <= NumOfImages; i++)
                {
                    _cards.Add(i.ToString());
                }
            }
            else if (cardType == CardType.RelatedImages)
            {
                for (var i = 1; i <= NumOfImages; i++)
                {
                    _cards.Add(i.ToString());
                }
            }

            var cardPool = new List<(string, int)>();

            for (var i = 0; i < BoardSize * BoardSize / 2; i++)
            {
                var card = _cards[_random.Next(_cards.Count)];
                cardPool.Add((card, 1));
                cardPool.Add((card, 2));

            }
            Shuffle(cardPool);

            for (var i = 0; i < BoardSize; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    var card = cardPool.First();
                    cardPool.Remove(card);
                    Cards.Add(new Card(card.Item1, card.Item2));
                }
            }
        }

        public bool Match()
        {
            bool isMatched = false;
            var revealedCards = Cards.Where(c => c.Revealed && !c.Matched);
            var matchedCards = Cards.Where(c1 => !c1.Matched && c1.Revealed && revealedCards.FirstOrDefault(c2 => !c2.Matched && c2.Revealed && c2 != c1 && c2.Text == c1.Text && (CardType != CardType.RelatedImages || c2.Index != c1.Index)) != null);
            if (matchedCards.Any())
            {
                var matchedCardsList = matchedCards.ToList();
                if (matchedCards.Count() > 2)
                {
                    
                    var card1 = matchedCardsList.FirstOrDefault(c1 => matchedCardsList.FirstOrDefault(c2 => c1 != c2 && c1.Text == c2.Text && c1.Index == c2.Index) != null);
                    var card2 = matchedCardsList.FirstOrDefault(c1 => matchedCardsList.FirstOrDefault(c2 => c1 != c2 && c1.Index == c2.Index) == null);
                    if (card1 != null && card2 != null)
                    {
                        card1.Matched = true;
                        card2.Matched = true;
                        isMatched = true;
                    }
                }
                else
                {
                    foreach (var card in matchedCardsList)
                    {
                        card.Matched = true;
                        isMatched = true;
                    }
                }
            }

            return isMatched || revealedCards.Count() == AllowToReveal;
        }

        private void Shuffle(List<(string, int)> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void InitializeRecords()
        {
            for (var level = 1; level <= 4; level ++)
            {
                for (var size = 4; size <= 10; size += 2)
                {
                    Records.Add((level, size, "0-9"), 999);
                    Records.Add((level, size, "0-15"), 999);
                    Records.Add((level, size, "A-Z"), 999);
                    Records.Add((level, size, "Images"), 999);
                    Records.Add((level, size, "Related Images"), 999);
                }
            }
        }

        public async Task SaveAsync(ILocalStorageService localStore)
        {
            var oldValue = await localStore.GetItemAsync<int>($"{Level}_{BoardSize}_{CardType}");
            if (oldValue == 0 || TimeCounter < oldValue)
            {
                await localStore.SetItemAsync($"{Level}_{BoardSize}_{CardType}", TimeCounter);
            }
        }

        public async Task LoadAsync(ILocalStorageService localStore)
        {
            for(var level = 1; level <= 4; level ++)
            {
                for (var size = 4; size <= 10; size += 2)
                {
                    var oldValue = await localStore.GetItemAsync<int>($"{level}_{size}_Decimal");
                    if (oldValue != 0)
                    {
                        Records[(level, size, "0-9")] = oldValue;
                    }
                    oldValue = await localStore.GetItemAsync<int>($"{level}_{size}_Hexadecimal");
                    if (oldValue != 0)
                    {
                        Records[(level, size, "0-15")] = oldValue;
                    }
                    oldValue = await localStore.GetItemAsync<int>($"{level}_{size}_Alphabet");
                    if (oldValue != 0)
                    {
                        Records[(level, size, "A-Z")] = oldValue;
                    }
                    oldValue = await localStore.GetItemAsync<int>($"{level}_{size}_Images");
                    if (oldValue != 0)
                    {
                        Records[(level, size, "Images")] = oldValue;
                    }
                    oldValue = await localStore.GetItemAsync<int>($"{level}_{size}_RelatedImages");
                    if (oldValue != 0)
                    {
                        Records[(level, size, "Related Images")] = oldValue;
                    }
                }
            }
        }
    }
}
