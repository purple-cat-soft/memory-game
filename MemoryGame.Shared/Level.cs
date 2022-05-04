namespace MemoryGame.Shared
{
  public class Level
  {
    public Level(int value, int columns, int rows)
    {
      Value = value;
      Columns = columns;
      Rows = rows;
    }

    public int Value { get; set; }
    public int Columns { get; }
    public int Rows { get; }
  }
}