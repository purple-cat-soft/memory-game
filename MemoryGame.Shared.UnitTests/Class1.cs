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
      var levels = levelProvider.GetLevels(10);

      foreach (var level in levels)
      {
        Console.Out.WriteLine($"{level.Rows}x{level.Columns}\t {level.Value}");
      }
    }
  }
}