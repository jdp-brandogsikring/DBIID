using DBIID.API.Client.Pages;
using DBIID.API.Components;
using DBIID.Application;
using DBIID.Infrastructure;
using DBIID.Application;
using DBIID.Domain;

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

builder.Services.AddControllers();


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
    options.DocumentFilter<IRequestSwaggerDocumentFilter>();
});

var app = builder.Build();

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

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DBIID.API.Client._Imports).Assembly);

app.Run();
