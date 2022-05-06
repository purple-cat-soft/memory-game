namespace MemoryGame.Shared
{
  public class Level
  {
    public Level(int rows, int columns)
    {
      Columns = columns;
      Rows = rows;
    }

    public int Value { get; set; }
    public int Columns { get; }
    public int Rows { get; }
  }
}