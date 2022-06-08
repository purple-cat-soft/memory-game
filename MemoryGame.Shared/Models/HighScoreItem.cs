using System.Text.Json.Serialization;

namespace MemoryGame.Models;

public class HighScoreItem
{
  [JsonPropertyName("L")]
  public int Level { get; set; }

  [JsonPropertyName("T")]
  public double TimeInSeconds { get; set; }

  [JsonPropertyName("TU")]
  public int TurnUps { get; set; }
}

