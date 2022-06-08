namespace MemoryGame.Models;

public class HighScore
{
  public List<HighScoreItem> Items { get; set; }

  public HighScore()
  {
    Items = new List<HighScoreItem>();
  }

  public HighScoreItem Add(int level, double timeInSeconds, int turnUps)
  {
    HighScoreItem highScoreItem = new HighScoreItem()
    {
      Level = level,
      TimeInSeconds = timeInSeconds,
      TurnUps = turnUps
    };

    Items.Add(highScoreItem);

    return highScoreItem;
  }
}

