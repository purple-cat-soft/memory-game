using System;
using System.Collections.Generic;

namespace MemoryGame.Shared
{
  public interface ILevelProvider
  {
    IEnumerable<Level> GetLevels(int maxRows);
  }
}
