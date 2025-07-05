using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MineSweeper;
using MineSweeper.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<GameService>();
builder.Services.AddScoped<PersistenceService>();
builder.Services.AddScoped<SoundService>();
builder.Services.AddScoped<AchievementService>();

await builder.Build().RunAsync();