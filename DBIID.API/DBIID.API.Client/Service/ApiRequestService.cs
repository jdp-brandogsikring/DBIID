using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DBIID.Application.Shared.Attributes;
using DBIID.Shared.Results;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public class ApiRequestService : IApiRequestService
{
    private readonly HttpClient _httpClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiRequestService(HttpClient httpClient, IServiceProvider serviceProvider)
    {
        _httpClient = httpClient;
        this._serviceProvider = serviceProvider;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// Sends an API request based on the Command or Query type.
    /// Automatically detects the response type from IRequest<TResponse>.
    /// </summary>
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        var validationResult = await ValidateRequest(request);
        if (!validationResult.IsSuccess)
        {
            string validationResultAsString = JsonSerializer.Serialize(validationResult); // ✅ Serialize valideringsfejl
            var validationError = JsonSerializer.Deserialize<TResponse>(validationResultAsString); // ✅ Deserialize valideringsfejl

            return validationError; // ✅ Returnér valideringsfejl uden at sende API-kald
        }

        var requestType = request.GetType();
        var attribute = requestType.GetCustomAttribute<HttpRequestAttribute>();

        if (attribute == null)
            throw new InvalidOperationException($"HttpRequestAttribute is missing on {requestType.Name}.");

        string url = BuildUrl("/api/" + attribute.Route, request);
        HttpMethod httpMethod = GetHttpMethod(attribute.Method);
        HttpRequestMessage httpRequest = new HttpRequestMessage(httpMethod, url);

        if (httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            Console.WriteLine("📦 Request JSON: " + json); // Debug
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        return await HandleResponse<TResponse>(response);
    }

    /// <summary>
    /// Validerer requestet med FluentValidation før API-kald.
    /// </summary>
    private async Task<Result<TResponse>> ValidateRequest<TResponse>(IRequest<TResponse> request)
    {
        // ✅ Hent validator for `request`, hvis den findes i DI-containeren
        var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
        var validatorObj = _serviceProvider.GetService(validatorType);

        if (validatorObj == null) return Result<TResponse>.Success(); // ✅ Ingen valideringsfejl, fortsæt

        var validator = (IValidator)validatorObj;
        var validationResult = await validator.ValidateAsync(new ValidationContext<object>(request));

        if (validationResult.IsValid) return Result<TResponse>.Success();

        string errorMessages = string.Join("\n", validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
        return Result<TResponse>.ValidationError(errorMessages) as Result<TResponse>; // ✅ Returnér valideringsfejl
    }

    /// <summary>
    /// Builds the URL by replacing placeholders with actual route parameters.
    /// </summary>
    private string BuildUrl(string route, object request)
    {
        var props = request.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

        var matches = Regex.Matches(route, "{(.*?)}");

        foreach (Match match in matches)
        {
            var key = match.Groups[1].Value; // fx "id"

            if (props.TryGetValue(key, out var prop))
            {
                var value = prop.GetValue(request)?.ToString() ?? string.Empty;
                route = route.Replace(match.Value, Uri.EscapeDataString(value));
            }
            else
            {
                // Leave {key} unchanged if no matching property
                Console.WriteLine($"⚠️ Property '{key}' not found on request object.");
            }
        }

        return route;
    }

    /// <summary>
    /// Maps the HttpMethodType enum to an actual HttpMethod.
    /// </summary>
    private HttpMethod GetHttpMethod(HttpMethodType method)
    {
        return method switch
        {
            HttpMethodType.GET => HttpMethod.Get,
            HttpMethodType.POST => HttpMethod.Post,
            HttpMethodType.PUT => HttpMethod.Put,
            HttpMethodType.DELETE => HttpMethod.Delete,
            _ => throw new NotImplementedException($"Unsupported HTTP method: {method}")
        };
    }

    /// <summary>
    /// Handles the response from the API dynamically.
    /// </summary>
    private async Task<TResponse> HandleResponse<TResponse>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error: {response.StatusCode}\n{errorContent}");
        }

        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions) ?? throw new Exception("Invalid response format.");
    }
}
