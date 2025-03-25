using DBIID.API.Client.Pages;
using DBIID.API.Components;
using DBIID.Application;
using DBIID.Infrastructure;
using DBIID.Application;
using DBIID.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using MediatR;
using DBIID.Application.Features.Users;
using FluentValidation;
using DBI.DIGI.Components;
using Blazored.LocalStorage;
using DBIID.Application.Features.Auth;
using DBIID.API.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// Inject the assembly reference dynamically
var assemblyReference = typeof(DBIID.Application.AssemblyReference).Assembly;

// Inject the assembly to DI for controller to use  
builder.Services.AddSingleton(assemblyReference);

// Validate routes using the injected assembly reference
RouteValidator.ValidateRoutes(assemblyReference); // Throws an exception if conflicts are found!


string dbConnectionString = builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
builder.Services.AddDbContext(dbConnectionString);
builder.Services.AddRepositories();
builder.Services.AddApplication();
builder.Services.AddAutoMapper();

// ✅ Automatisk registrering af alle FluentValidation validators i projektet
builder.Services.AddValidatorsFromAssemblyContaining<DBIID.Shared.AssemblyReference>();

// ✅ MediatR med FluentValidation Pipeline
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DBIID.Shared.AssemblyReference>());

// ✅ Tilføj MediatR valideringspipeline
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


builder.Services.AddControllers();


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


// Swagger-konfiguration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "Automatisk genereret REST API med CQRS og MediatR"
    });
    options.DocumentFilter<RequestSwaggerDocumentFilter>();
});

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"];
var jwtIssuer = jwtSettings["Issuer"];
var jwtAudience = jwtSettings["Audience"];

if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
{
    throw new Exception("JWT Key must be at least 32 characters long.");
}

// ✅ Ensure Correct JWT Validation
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = signingKey
        };

        // ✅ Log Authentication Failures
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"🔴 Authentication Failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"🔴 JWT Challenge Triggered: {context.Error}, {context.ErrorDescription}");
                return Task.CompletedTask;
            },
            OnForbidden = context =>
            {
                Console.WriteLine($"🔴 JWT Forbidden: {context.HttpContext.User.Identity?.Name} is not allowed.");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSingleton<JwtService>();

builder.Services.AddScoped<ISendOTPService, SendOTPService>();

// DIGI Design
builder.Services.RegisterDependencies();

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        Console.WriteLine("RAW BODY: " + body);

        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Authentication Error: {ex.Message}");
        throw;
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAntiforgery();

// Swagger UI kun på /swagger
app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger"; // Sørger for at Swagger UI kun er på /swagger
});

app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseAuthentication(); // First, check authentication
app.UseAuthorization();  // Then, enforce authorization

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DBIID.API.Client._Imports).Assembly);



app.Run();
