using MemoryGame.Shared;
using Microsoft.AspNetCore.Components;

namespace MemoryGame.Razor.Shared;

public partial class MainLayout
{
  [Inject]
  public IApplicationService ApplicationService { get; set; }

  private string Title => ApplicationService.Title;

  protected override void OnAfterRender(bool firstRender)
  {
    if (firstRender)
    {
      ApplicationService.OnTitleChanged += OnTitleChanged;
    }
  }

  private void OnTitleChanged(object sender, EventArgs e)
  {
    InvokeAsync(() => StateHasChanged());
  }
}