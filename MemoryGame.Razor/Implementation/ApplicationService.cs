namespace MemoryGame.Shared;

public class ApplicationService : IApplicationService
{
  public event EventHandler OnTitleChanged;
  
  public ApplicationService()
  {
    Title = "Memory Game";
  }

  public string Title { get; set; }
  
  public void SetTitle(string title)
  {
    Title = title;

    OnTitleChanged?.Invoke(this, EventArgs.Empty);
  }
}