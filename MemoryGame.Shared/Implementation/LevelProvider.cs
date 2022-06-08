using Blazored.LocalStorage;
using MemoryGame.Models;

namespace MemoryGame.Shared
{
  public class LevelProvider : ILevelProvider
  {
    private readonly ILocalStorageService mStorageService;

    public LevelProvider(ILocalStorageService storageService)
    {
      mStorageService = storageService;
    }

    private async Task<Level[]> BuildLevels()
    {
      var maxLevelFinished = await GetMaxLevelFinished(mStorageService);
      var highScore = await GetHighScore(mStorageService);

      var levels = GetLevelsInternal().OrderBy(x => x.Rows * x.Columns).ToArray();
      int levelValue = 1;


      foreach (Level level in levels)
      {
        level.SetValue(levelValue);
        
        if (levelValue == 1 || levelValue <= maxLevelFinished + 1)
        {
          level.Enable();
        }

        level.SetHighScore(highScore.Items.SingleOrDefault(x => x.Level == level.Value));

        levelValue++;
      }

      return levels;
    }

    public async Task<Level> GetLevel(int levelValue)
    {
      var levels = await GetLevels();
      return levels.Single(x => x.Value == levelValue);
    }

    public Task<Level[]> GetLevels()
    {
      return BuildLevels();
    }

    public async Task LevelFinished(Level level, TimeSpan elapsedTime)
    {
      await UpdateMaxLevelFinished(mStorageService, level);
      await UpdateHighScore(mStorageService, level, elapsedTime.TotalSeconds, 0);
    }

    private IEnumerable<Level> GetLevelsInternal()
    {
      yield return new Level(2, 2);
      yield return new Level(3, 2);
      yield return new Level(4, 3);
      yield return new Level(4, 4);
      yield return new Level(5, 4);
      yield return new Level(6, 4);
      yield return new Level(6, 5);
      yield return new Level(6, 6);
      yield return new Level(7, 6);
      yield return new Level(8, 6);
      yield return new Level(8, 7);
      yield return new Level(8, 8);
      yield return new Level(9, 8);
      yield return new Level(10, 8);
      yield return new Level(10, 9);
      yield return new Level(10, 10);
    }

    private static async Task<int> GetMaxLevelFinished(ILocalStorageService storageService)
    {
      return await storageService.GetItemAsync<int>("Level");
    }

    private static async Task UpdateHighScore(ILocalStorageService storageService, Level level, double timeInSeconds, int turnUps)
    {
      var highScore = await GetHighScore(storageService);
      var highScoreItem = highScore.Items.SingleOrDefault(x => x.Level == level.Value);

      if (highScoreItem == null)
      {
        highScoreItem = highScore.Add(level.Value, double.MaxValue, 0);
      }

      if (timeInSeconds < highScoreItem.TimeInSeconds)
      {
        highScoreItem.TimeInSeconds = timeInSeconds;

        var highScoreAsString = System.Text.Json.JsonSerializer.Serialize(highScore);
        await storageService.SetItemAsync("HighScore", highScoreAsString);
      }
    }

    private static async Task<HighScore> GetHighScore(ILocalStorageService storageService)
    {
      string highScoreAsString = await storageService.GetItemAsync<string>("HighScore");
      var highScore = highScoreAsString == null ? new HighScore() : System.Text.Json.JsonSerializer.Deserialize<HighScore>(highScoreAsString);

      return highScore;
    }

    private static async Task UpdateMaxLevelFinished(ILocalStorageService storageService, Level level)
    {
      int maxLevelFinished = await GetMaxLevelFinished(storageService);

      if (level.Value > maxLevelFinished)
      {
        await storageService.SetItemAsync("Level", level.Value);
      }
    }
  }
}