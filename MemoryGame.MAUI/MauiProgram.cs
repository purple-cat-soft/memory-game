using MemoryGame.Razor;

namespace MemoryGame.MAUI
{
  public static class MauiProgram
  {
    public static MauiApp CreateMauiApp()
    {
      var builder = MauiApp.CreateBuilder();
      builder.UseMauiApp<App>();
      builder.Services.AddMauiBlazorWebView();
      builder.Services.AddCommon();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif


      return builder.Build();
    }
  }
}