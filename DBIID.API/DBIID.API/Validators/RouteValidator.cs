using DBIID.Application.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class RouteValidator
{
    public static void ValidateRoutes(Assembly assembly)
    {
        var requests = assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<HttpRequestAttribute>() != null)
            .Select(t => new
            {
                Type = t,
                Attribute = t.GetCustomAttribute<HttpRequestAttribute>()
            })
            .ToList();

        var routeMap = new Dictionary<string, List<string>>(); // Key: "METHOD:route", Value: List of IRequest types
        var errors = new List<string>();

        foreach (var request in requests)
        {
            string route = request.Attribute.Route.ToLower();
            HttpMethodType method = request.Attribute.Method;
            string key = $"{method}:{route}";

            if (!routeMap.ContainsKey(key))
            {
                routeMap[key] = new List<string>();
            }
            else
            {
                errors.Add($"Route conflict detected: The route [{TranslateMethod(method)} {route}] is used by multiple request handlers: " +
                           $"{string.Join(", ", routeMap[key])} and {request.Type.Name}.");
            }

            routeMap[key].Add(request.Type.Name);
        }

        // Throw an exception if conflicts exist
        if (errors.Any())
        {
            throw new InvalidOperationException($"Route validation failed due to conflicts:\n{string.Join("\n", errors)}");
        }

        Console.WriteLine("All routes are unique and valid.");
    }

    public static string TranslateMethod(HttpMethodType method)
    {
        return method switch
        {
            HttpMethodType.GET => "GET",
            HttpMethodType.POST => "POST",
            HttpMethodType.PUT => "PUT",
            HttpMethodType.DELETE => "DELETE",
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
        };
    }
}
