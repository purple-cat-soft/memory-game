using System.Collections.Generic;
using System.Linq;

namespace MemoryGame.Shared
{
  public class LevelProvider : ILevelProvider
  {
    public LevelProvider()
    {
      var levels = GetLevels().OrderBy(x=>x.Rows*x.Columns).ToArray();
      int levelValue = 0;
      foreach (Level level in levels)
      {
        level.Value = ++levelValue;
      }

      Levels = levels;
    }

    public IList<Level> Levels { get; }

    public Level GetLevel(int levelValue)
    {
      return Levels.Single(x => x.Value == levelValue);
    }

    private IEnumerable<Level> GetLevels()
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
  }
}