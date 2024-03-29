﻿namespace MemoryGame.Shared;

public class ApplicationService : IApplicationService
{
  public event EventHandler OnTitleChanged;
  
  public ApplicationService()
  {
    Title = "Select the level";
  }

  public string Title { get; set; }
  
  public void SetTitle(string title)
  {
    Title = title;

    OnTitleChanged?.Invoke(this, EventArgs.Empty);
  }
}