using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using DBIID.Application.Common.Attributes;

[ApiController]
[Route("api")]
[ApiExplorerSettings(IgnoreApi = true)] // ✅ Skjuler denne controller fra Swagger
public class DynamicRestController : ControllerBase
{
    private readonly IMediator _mediator;

    public DynamicRestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet, HttpPost, HttpPut, HttpDelete]
    [Route("{*url}")]
    public async Task<IActionResult> HandleRequest([FromRoute] string url, [FromBody] object requestBody = null)
    {
        var requestType = FindRequestType(url, Request.Method);
        if (requestType == null)
            return NotFound("No matching request found.");

        // 🔹 1️⃣ Opret request-instans
        var requestInstance = Activator.CreateInstance(requestType);
        var errors = new List<string>();

        // 🔹 2️⃣ Hent URL-variabler fra ruten
        var attribute = requestType.GetCustomAttribute<HttpRequestAttribute>();
        var paramNames = ExtractRouteParameters(attribute.Route);
        var paramValues = ExtractValuesFromUrl(attribute.Route, url);

        // 🔹 3️⃣ Map URL-variabler til request-properties (CASE-INSENSITIVE + VALIDERING)
        var properties = requestType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .ToDictionary(p => p.Name.ToLower(), p => p);

        foreach (var param in paramNames)
        {
            string paramLower = param.ToLower();
            if (properties.ContainsKey(paramLower))
            {
                var prop = properties[paramLower];
                if (paramValues.ContainsKey(paramLower))
                {
                    try
                    {
                        object convertedValue = Convert.ChangeType(paramValues[paramLower], prop.PropertyType);
                        prop.SetValue(requestInstance, convertedValue);
                    }
                    catch (Exception)
                    {
                        errors.Add($"Parameter '{param}' could not be converted to {prop.PropertyType.Name}.");
                    }
                }
            }
            else
            {
                errors.Add($"Parameter '{param}' is not recognized.");
            }
        }

        // 🔹 4️⃣ Returner fejl hvis der er nogen
        if (errors.Any())
        {
            return BadRequest(new { errors });
        }

        // 🔹 5️⃣ Map body-variabler, hvis ikke GET
        if (Request.Method != "GET" && requestBody != null)
        {
            var bodyData = JsonSerializer.Deserialize(requestBody.ToString(), requestType);
            foreach (var prop in requestType.GetProperties())
            {
                var value = prop.GetValue(bodyData);
                if (value != null) prop.SetValue(requestInstance, value);
            }
        }

        var response = await _mediator.Send(requestInstance);
        return response != null ? Ok(response) : NoContent();
    }

    private static Type FindRequestType(string url, string method)
    {
        var assembly = typeof(DBIID.Application.AssemblyReference).Assembly;
        var allRequests = assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<HttpRequestAttribute>() != null)
            .Select(t => new
            {
                Type = t,
                Attribute = t.GetCustomAttribute<HttpRequestAttribute>()
            })
            .Where(t => t.Attribute.Method.Equals(method, StringComparison.OrdinalIgnoreCase))
            .ToList();

        // 🔹 1️⃣ Eksakte ruter (f.eks. "User/All") matcher først
        var exactMatch = allRequests.FirstOrDefault(t => t.Attribute.Route.Equals(url, StringComparison.OrdinalIgnoreCase));
        if (exactMatch != null) return exactMatch.Type;

        // 🔹 2️⃣ Matcher `{}`-ruter (f.eks. "User/{id}")
        foreach (var request in allRequests.OrderBy(t => t.Attribute.Route.Count(c => c == '{')))
        {
            string routePattern = "^" + ConvertRouteToRegex(request.Attribute.Route) + "$";

            if (Regex.IsMatch(url, routePattern, RegexOptions.IgnoreCase))
            {
                return request.Type;
            }
        }

        return null;
    }

    // 🔹 Ekstraherer `{}`-parametre fra ruten
    private static List<string> ExtractRouteParameters(string route)
    {
        return Regex.Matches(route, @"{(\w+)}")
            .Select(m => m.Groups[1].Value)
            .ToList();
    }

    // 🔹 Matcher `{}`-parametre med værdier fra URL'en
    private static Dictionary<string, string> ExtractValuesFromUrl(string route, string url)
    {
        string pattern = "^" + ConvertRouteToRegex(route) + "$";
        var match = Regex.Match(url, pattern, RegexOptions.IgnoreCase);
        var paramValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var groupName in match.Groups.Keys.Where(k => k != "0"))
        {
            paramValues[groupName] = match.Groups[groupName].Value;
        }

        return paramValues;
    }

    // 🔹 Konverter `{param}` til regex
    private static string ConvertRouteToRegex(string route)
    {
        return route
            .Replace("/", "\\/") // Escape slashes
            .Replace("{", "(?<") // Start named group
            .Replace("}", ">[^/]+)"); // Match alt undtagen "/"
    }
}
