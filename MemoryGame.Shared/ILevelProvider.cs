using System.Collections.Generic;

namespace MemoryGame.Shared
{
  public interface ILevelProvider
  {
    IList<Level> Levels { get; }
    Level GetLevel(int levelValue);
  }
}
