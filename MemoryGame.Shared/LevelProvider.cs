using System.Collections.Generic;
using System.Linq;

namespace MemoryGame.Shared
{
  public class LevelProvider : ILevelProvider
  {
    public IEnumerable<Level> GetLevels(int maxRows)
    {
      var levels = new List<KeyValuePair<int,int>>();

      for (int rows = 2; rows <= maxRows; rows++)
      {
        for (int columns = 2; columns <= rows; columns++)
        {
          if (columns * rows % 2 == 0)
          {
            levels.Add(new KeyValuePair<int, int>(columns,rows));
          }
        }
      }

      int levelValue = 0;
      foreach (var level in levels.OrderBy(x=>x.Key*x.Value))
      {
        yield return new Level(++levelValue, level.Key, level.Value);
      }
    }
  }
}