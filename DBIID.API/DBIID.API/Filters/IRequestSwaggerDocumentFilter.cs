using DBIID.Application.Common.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class IRequestSwaggerDocumentFilter : IDocumentFilter
{
    public Assembly _assembly { get; }

    public IRequestSwaggerDocumentFilter(Assembly assembly)
    {
        _assembly = assembly;
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

            var operation = new OpenApiOperation
            {
                Summary = $"Dynamisk endpoint for {requestType.Name}",
                Responses = new OpenApiResponses
                {
                    ["200"] = new OpenApiResponse { Description = "Success" }
                },
                Parameters = new List<OpenApiParameter>()
            };

            // 🔹 Håndter `{}`-parametre i URL'en og find deres datatype
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

            // 🔹 Fjern request body for DELETE requests
            if (method != "get" && method != "delete")
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = context.SchemaGenerator.GenerateSchema(requestType, context.SchemaRepository)
                        }
                    }
                };
            }

            if (!swaggerDoc.Paths.ContainsKey(path))
            {
                swaggerDoc.Paths.Add(path, new OpenApiPathItem());
            }

            swaggerDoc.Paths[path].Operations[GetOperationType(method)] = operation;
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

    // 🔹 Konverter .NET datatyper til OpenAPI datatyper
    private static string GetOpenApiType(Type type)
    {
        return type == typeof(int) ? "integer" :
               type == typeof(bool) ? "boolean" :
               type == typeof(double) || type == typeof(float) ? "number" :
               "string"; // Standard fallback
    }
}
