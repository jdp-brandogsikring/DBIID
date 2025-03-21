using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FluentValidation;
using DBI.DIGI.Components;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Inject the Shared Assembly containing Commands & Queries
var sharedAssembly = typeof(DBIID.Shared.AssemblyReference).Assembly;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7128/api") });
builder.Services.AddSingleton(sharedAssembly);
builder.Services.AddScoped<IApiRequestService, ApiRequestService>();

// Register Authentication State Provider
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthStartupService>();

// ✅ Registrér alle validators
builder.Services.AddValidatorsFromAssemblyContaining<DBIID.Shared.AssemblyReference>();

// DIGI Design
builder.Services.RegisterDependencies();

await builder.Build().RunAsync();
