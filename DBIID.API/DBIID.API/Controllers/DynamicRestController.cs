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
    private readonly Assembly _assembly;

    public DynamicRestController(IMediator mediator, Assembly assembly)
    {
        _mediator = mediator;
        _assembly = assembly;
    }

    [HttpGet, HttpPost, HttpPut, HttpDelete]
    [Route("{*url}")]
    public async Task<IActionResult> HandleRequest([FromRoute] string url, [FromBody] object requestBody = null)
    {
        var requestType = FindRequestType(url, Request.Method);
        if (requestType == null)
            return NotFound("No matching request found.");

        var requestInstance = Activator.CreateInstance(requestType);
        var errors = new List<string>();

        // 🔹 Extract URL parameters
        var attribute = requestType.GetCustomAttribute<HttpRequestAttribute>();
        var paramNames = ExtractRouteParameters(attribute.Route);
        var paramValues = ExtractValuesFromUrl(attribute.Route, url);

        // 🔹 Map URL parameters to request properties (case-insensitive)
        var properties = requestType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .ToDictionary(p => p.Name.ToLower(), p => p);

        var requestBodyDict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        // 🔹 Map URL params and validate type conversion
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

                        // Save mapped URL values for later validation
                        requestBodyDict[paramLower] = convertedValue;
                    }
                    catch (Exception)
                    {
                        errors.Add($"Parameter '{param}' in the URL could not be converted to {prop.PropertyType.Name}.");
                    }
                }
            }
            else
            {
                errors.Add($"Parameter '{param}' is not recognized.");
            }
        }

        // 🔹 Deserialize and validate the request body if not GET or DELETE
        if (Request.Method != "GET" && Request.Method != "DELETE" && requestBody != null)
        {
            var bodyData = JsonSerializer.Deserialize(requestBody.ToString(), requestType);
            foreach (var prop in requestType.GetProperties())
            {
                var value = prop.GetValue(bodyData);
                if (value != null)
                {
                    prop.SetValue(requestInstance, value);
                    requestBodyDict[prop.Name.ToLower()] = value;
                }
            }

            // 🔹 Compare URL values with Body values
            foreach (var param in paramNames)
            {
                string paramLower = param.ToLower();
                if (requestBodyDict.ContainsKey(paramLower) && paramValues.ContainsKey(paramLower))
                {
                    var urlValue = requestBodyDict[paramLower]?.ToString();
                    var bodyValue = paramValues[paramLower]?.ToString();

                    if (!string.Equals(urlValue, bodyValue, StringComparison.OrdinalIgnoreCase))
                    {
                        errors.Add($"Mismatch: '{param}' in URL ('{bodyValue}') does not match '{param}' in body ('{urlValue}').");
                    }
                }
            }
        }

        // 🔹 Return errors if any exist
        if (errors.Any())
        {
            return BadRequest(new { errors });
        }

        var response = await _mediator.Send(requestInstance);
        return response != null ? Ok(response) : NoContent();
    }

    private Type FindRequestType(string url, string method)
    {
        var allRequests = _assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<HttpRequestAttribute>() != null)
            .Select(t => new
            {
                Type = t,
                Attribute = t.GetCustomAttribute<HttpRequestAttribute>()
            })
            .Where(t => t.Attribute.Method.Equals(method, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var exactMatch = allRequests.FirstOrDefault(t => t.Attribute.Route.Equals(url, StringComparison.OrdinalIgnoreCase));
        if (exactMatch != null) return exactMatch.Type;

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

    private static List<string> ExtractRouteParameters(string route)
    {
        return Regex.Matches(route, @"{(\w+)}")
            .Select(m => m.Groups[1].Value)
            .ToList();
    }

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

    private static string ConvertRouteToRegex(string route)
    {
        return route
            .Replace("/", "\\/") // Escape slashes
            .Replace("{", "(?<") // Start named group
            .Replace("}", ">[^/]+)"); // Match everything except "/"
    }
}