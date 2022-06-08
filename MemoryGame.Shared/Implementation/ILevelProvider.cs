using System.Collections.Generic;

namespace MemoryGame.Shared
{
  public interface ILevelProvider
  {
    Task<Level> GetLevel(int levelValue);
    Task LevelFinished(Level level, TimeSpan elapsed);
    Task<Level[]> GetLevels();
  }
}
