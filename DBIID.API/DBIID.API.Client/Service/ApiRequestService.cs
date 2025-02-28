using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using DBIID.Application.Shared.Attributes;
using MediatR;

public class ApiRequestService : IApiRequestService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiRequestService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// Sends an API request based on the Command or Query type.
    /// Automatically detects the response type from IRequest<TResponse>.
    /// </summary>
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        var requestType = request.GetType();
        var attribute = requestType.GetCustomAttribute<HttpRequestAttribute>();

        if (attribute == null)
            throw new InvalidOperationException($"HttpRequestAttribute is missing on {requestType.Name}.");

        string url = BuildUrl("/api/" + attribute.Route, request);
        HttpMethod httpMethod = GetHttpMethod(attribute.Method);
        HttpRequestMessage httpRequest = new HttpRequestMessage(httpMethod, url);

        if (httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put)
        {
            httpRequest.Content = JsonContent.Create(request);
        }

        HttpResponseMessage response = await _httpClient.SendAsync(httpRequest);
        return await HandleResponse<TResponse>(response);
    }

    /// <summary>
    /// Builds the URL by replacing placeholders with actual route parameters.
    /// </summary>
    private string BuildUrl(string route, object request)
    {
        foreach (var prop in request.GetType().GetProperties())
        {
            string key = $"{{{prop.Name}}}";
            string value = prop.GetValue(request)?.ToString() ?? string.Empty;
            route = route.Replace(key, Uri.EscapeDataString(value));
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
