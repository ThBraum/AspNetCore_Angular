using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProEventos.API;

public class SwaggerOrderFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var orderedControllers = new List<string> { "User", "Lote", "Evento" };

        var reorderedPaths = new Dictionary<string, OpenApiPathItem>();
        foreach (var controllerName in orderedControllers)
        {
            var pathsForController = swaggerDoc.Paths
                .Where(path => path.Key.Contains($"/api/{controllerName}"))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var path in pathsForController)
            {
                reorderedPaths.Add(path.Key, path.Value);
            }
        }

        swaggerDoc.Paths = new OpenApiPaths();
        foreach (var path in reorderedPaths)
        {
            swaggerDoc.Paths.Add(path.Key, path.Value);
        }
    }
}

