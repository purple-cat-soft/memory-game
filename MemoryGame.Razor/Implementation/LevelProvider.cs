using Blazored.LocalStorage;

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
      var levels = GetLevelsInternal().OrderBy(x => x.Rows * x.Columns).ToArray();
      int levelValue = 1;

      foreach (Level level in levels)
      {
        level.SetValue(levelValue);

        if (levelValue == 1 || levelValue <= maxLevelFinished + 1)
        {
          level.Enable();
        }

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

    public async Task LevelFinished(Level level)
    {
      int maxLevelFinished = await GetMaxLevelFinished(mStorageService);

      if (level.Value > maxLevelFinished)
      {
        await SetMaxLevelFinished(mStorageService, level);
      }
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

    private static Task<int> GetMaxLevelFinished(ILocalStorageService storageService)
    {
      return storageService.GetItemAsync<int>("Level").AsTask();
    }

    private static Task SetMaxLevelFinished(ILocalStorageService storageService, Level level)
    {
      return storageService.SetItemAsync("Level", level.Value).AsTask();
    }
  }
}