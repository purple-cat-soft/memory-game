using Blazored.LocalStorage;
using MemoryGame.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryGame.Razor
{
  public static class ServiceCollectionExtensions
  {
    public static void AddCommon(this IServiceCollection services)
    {
      services.AddBlazoredLocalStorage();
      services.AddScoped<ILevelProvider, LevelProvider>();
      services.AddScoped<IApplicationService, ApplicationService>();
    }
  }
}
