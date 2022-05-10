using Blazored.LocalStorage;
using MemoryGame;
using MemoryGame.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ILevelProvider,LevelProvider>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();

await builder.Build().RunAsync();
