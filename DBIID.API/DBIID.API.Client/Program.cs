using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Inject the Shared Assembly containing Commands & Queries
var sharedAssembly = typeof(DBIID.Shared.AssemblyReference).Assembly;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7128/api") });
builder.Services.AddSingleton(sharedAssembly);
builder.Services.AddScoped<IApiRequestService, ApiRequestService>();

await builder.Build().RunAsync();
