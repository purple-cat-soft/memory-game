using Blazored.LocalStorage;
using MemoryGame.Razor.Implementation;
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
      services.AddScoped<ICardFactoryProvider, CardFactoryProvider>();
      services.AddScoped<ICardFactory, NumbersCardFactory>();
      services.AddScoped<ICardFactory, OperationsCardFactory>();
    }
  }
}
