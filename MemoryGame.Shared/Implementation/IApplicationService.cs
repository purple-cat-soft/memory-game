namespace MemoryGame.Shared;

public interface IApplicationService
{
  string Title { get; }
  void SetTitle(string title);
  event EventHandler OnTitleChanged;
}