namespace MemoryGame.Shared
{
  public class Level
  {
    public Level(int rows, int columns)
    {
      Columns = columns;
      Rows = rows;
    }

    public int Value { get; private set; }
    public int Columns { get; }
    public int Rows { get; }
    public bool IsEnabled { get; private set; }

    public void SetValue(int value)
    {
      Value = value;
    }

    public void Enable()
    {
      IsEnabled=true;
    }
  }
}