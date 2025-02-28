using DBIID.Application.Common.Attributes;
using MediatR;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class RequestSwaggerDocumentFilter : IDocumentFilter
{
    private readonly Assembly _assembly;

    public RequestSwaggerDocumentFilter(Assembly assembly)
    {
        _assembly = assembly; // Injected assembly reference
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var requestTypes = _assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<HttpRequestAttribute>() != null)
            .ToList();

        foreach (var requestType in requestTypes)
        {
            var attr = requestType.GetCustomAttribute<HttpRequestAttribute>();
            if (attr == null) continue;

            var path = $"/api/{attr.Route}";
            var method = attr.Method.ToLower();
            var resourceGroup = GetResourceGroup(attr.Route);
            var responseType = GetIRequestResponseType(requestType);

            var operation = new OpenApiOperation
            {
                Summary = $"Dynamically generated endpoint for {requestType.Name}",
                Tags = new List<OpenApiTag> { new OpenApiTag { Name = resourceGroup } },
                Responses = new OpenApiResponses
                {
                    ["200"] = new OpenApiResponse
                    {
                        Description = "Success",
                        Content = responseType != null ? new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/json"] = new OpenApiMediaType
                            {
                                Schema = context.SchemaGenerator.GenerateSchema(responseType, context.SchemaRepository)
                            }
                        } : null
                    },
                    ["400"] = new OpenApiResponse { Description = "Bad Request" },
                    ["404"] = new OpenApiResponse { Description = "Not Found" }
                },
                Parameters = new List<OpenApiParameter>()
            };

            // 🔹 Extract `{}` parameters from the route and match their types
            var paramNames = ExtractRouteParameters(attr.Route);
            var properties = requestType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .ToDictionary(p => p.Name.ToLower(), p => p);

            foreach (var param in paramNames)
            {
                string paramLower = param.ToLower();
                if (properties.ContainsKey(paramLower))
                {
                    var prop = properties[paramLower];

                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = param,
                        In = ParameterLocation.Path,
                        Required = true,
                        Schema = new OpenApiSchema { Type = GetOpenApiType(prop.PropertyType) }
                    });
                }
            }

            // 🔹 Manually generate request body schema instead of using `$ref`
            if (method != "get" && method != "delete")
            {
                var schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = properties.ToDictionary(
                        p => p.Value.Name,
                        p => new OpenApiSchema { Type = GetOpenApiType(p.Value.PropertyType) }
                    ),
                    Required = properties.Values.Select(p => p.Name).ToHashSet()
                };

                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType { Schema = schema }
                    }
                };
            }

            if (!swaggerDoc.Paths.ContainsKey(path))
            {
                swaggerDoc.Paths.Add(path, new OpenApiPathItem());
            }

            swaggerDoc.Paths[path].Operations[GetOperationType(method)] = operation;
        }

        // 🔹 Remove Command and Query classes from Swagger's schema section
        var requestTypeNames = requestTypes.Select(t => t.Name).ToHashSet();
        foreach (var key in context.SchemaRepository.Schemas.Keys.ToList())
        {
            if (requestTypeNames.Contains(key))
            {
                context.SchemaRepository.Schemas.Remove(key);
            }
        }
    }

    private static OperationType GetOperationType(string method)
    {
        return method switch
        {
            "get" => OperationType.Get,
            "post" => OperationType.Post,
            "put" => OperationType.Put,
            "delete" => OperationType.Delete,
            _ => throw new NotImplementedException()
        };
    }

    private static List<string> ExtractRouteParameters(string route)
    {
        return System.Text.RegularExpressions.Regex.Matches(route, @"{(\w+)}")
            .Select(m => m.Groups[1].Value)
            .ToList();
    }

    private static string GetOpenApiType(Type type)
    {
        return type == typeof(int) ? "integer" :
               type == typeof(bool) ? "boolean" :
               type == typeof(double) || type == typeof(float) ? "number" :
               "string"; // Default fallback
    }

    private static Type GetIRequestResponseType(Type requestType)
    {
        var interfaceType = requestType.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>));

        return interfaceType?.GetGenericArguments().FirstOrDefault();
    }

    private static string GetResourceGroup(string route)
    {
        var firstSegment = route.Split('/').FirstOrDefault();
        return !string.IsNullOrEmpty(firstSegment) ? $"{firstSegment}Controller" : "General";
    }
}