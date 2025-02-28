using Microsoft.AspNetCore.Components;
using System.Reflection;
using System.Text.RegularExpressions;

public class RouteAttributeTests
{
    private static readonly string ProjectRootPath = GetProjectRootPath();

    [Fact]
    public void AllComponents_ShouldUseRoutePathsConstant()
    {
        var assembly = Assembly.Load(typeof(DBIID.API.Client.AssemblyReference).Namespace);
        var componentTypes = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ComponentBase)))
            .ToList();

        var validRoutes = GetAllRoutePathConstants();

        foreach (var type in componentTypes)
        {
            var routeAttributes = type.GetCustomAttributes<RouteAttribute>().ToList();

            foreach (var routeAttribute in routeAttributes)
            {
                var routeValue = routeAttribute.Template;

                Assert.True(validRoutes.Contains(routeValue),
                    $"Component {type.Name} uses a hardcoded route: \"{routeValue}\". Use const from RoutePaths.");
            }
        }
    }

    [Fact]
    public void RazorFiles_ShouldNotUseHardcodedPageDirectives()
    {
        var pagesFolder = Path.Combine(ProjectRootPath, "Pages");
        var razorFiles = Directory.GetFiles(pagesFolder, "*.razor", SearchOption.AllDirectories);
        var pageDirectiveRegex = new Regex(@"@page\s+""(\/[^\s]*)""", RegexOptions.Compiled);

        foreach (var file in razorFiles)
        {
            var content = File.ReadAllText(file);
            var matches = pageDirectiveRegex.Matches(content);

            foreach (Match match in matches)
            {
                Assert.False(true,
                    $"File: {file} uses a hardcoded @page route: {match.Groups[1].Value}. Use const from RoutePaths. @attribute [Route(RoutePaths.X)]\r\n ");
            }
        }
    }

    private static string[] GetAllRoutePathConstants()
    {
        var routePathType = typeof(RoutePaths); 
        return routePathType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.FieldType == typeof(string))
            .Select(f => f.GetValue(null)?.ToString())
            .Where(value => value != null) 
            .ToArray();
    }

    private static string GetProjectRootPath()
    {
        var testBinPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (testBinPath == null)
        {
            throw new InvalidOperationException("Could not find the test project's bin directory.");
        }

        var binFolder = new DirectoryInfo(testBinPath);

        var blazorAssemblyFile = binFolder
            .GetFiles("*.dll", SearchOption.AllDirectories)
            .FirstOrDefault(f => f.Name.Contains("Client") || f.Name.Contains("Api.Client"));

        if (blazorAssemblyFile == null)
        {
            throw new FileNotFoundException("Could not find the Blazor WebAssembly Assembly in the test bin directory.");
        }

        var blazorBinPath = blazorAssemblyFile.DirectoryName;
        var directory = new DirectoryInfo(blazorBinPath);

        while (directory != null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, "DBIID.API", "DBIID.API.Client", "Pages")))
            {
                return Path.Combine(directory.FullName, "DBIID.API", "DBIID.API.Client");
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("The Blazor project root directory was not found. Ensure your project structure is correct.");
    }

}