using NUnit.Framework;

namespace MemoryGame.Shared.UnitTests
{
  [TestFixture]
  public class LevelProviderTests
  {
    [Test]
    public void TestGetLevels()
    {
      var levelProvider = new LevelProvider();

      foreach (var level in levelProvider.Levels)
      {
        Console.Out.WriteLine($"{level.Rows}x{level.Columns}\t{level.Rows * level.Columns}");
      }
    }
  }
}