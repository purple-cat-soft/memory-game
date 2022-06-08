using MemoryGame.Models;

namespace MemoryGame.Shared
{
  public class Level
  {
    public Level(int rows, int columns)
    {
      Columns = columns;
      Rows = rows;
    }

    public int Value { get; private set; }
    public int Columns { get; }
    public int Rows { get; }
    public bool IsEnabled { get; private set; }
    public string HighScore { get; private set; }

    public void SetValue(int value)
    {
      Value = value;
    }

    public void Enable()
    {
      IsEnabled = true;
    }

    internal void SetHighScore(HighScoreItem highScoreItem)
    {
      if (highScoreItem != null)
      {
        var timeSpan = TimeSpan.FromSeconds(highScoreItem.TimeInSeconds);
        HighScore = $"{timeSpan.TotalMinutes:00}:{timeSpan.TotalSeconds:00}";
      }
      else
      {
        if (IsEnabled)
        {
          HighScore = "-:-";
        }
      }
    }
  }
}