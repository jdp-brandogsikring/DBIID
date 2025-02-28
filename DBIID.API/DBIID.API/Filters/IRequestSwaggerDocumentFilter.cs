using DBIID.Application.Common.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class IRequestSwaggerDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var assemblies = new List<Assembly>
        {
            Assembly.GetExecutingAssembly(),
            typeof(DBIID.Application.AssemblyReference).Assembly
        };

        var requestTypes = assemblies
            .SelectMany(a => a.GetTypes())
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

            // ✅ Håndter `{}`-parametre i URL'en
            var paramNames = ExtractRouteParameters(attr.Route);
            foreach (var param in paramNames)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = param,
                    In = ParameterLocation.Path,
                    Required = true,
                    Schema = new OpenApiSchema { Type = "string" }
                });
            }

            // ✅ Tilføj request body for POST/PUT, men IKKE for GET
            if (method != "get")
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
        return route.Split('/')
            .Where(part => part.StartsWith("{") && part.EndsWith("}"))
            .Select(part => part.Trim('{', '}'))
            .ToList();
    }
}
