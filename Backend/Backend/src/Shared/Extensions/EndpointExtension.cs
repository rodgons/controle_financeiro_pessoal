using System.Reflection;
using Backend.Shared.Interfaces;

namespace Backend.Config.Extensions;

public static class EndpointExtension
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpointTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false }
                        && t.GetInterfaces().Contains(typeof(IEndpoint)));

        foreach (var type in endpointTypes)
        {
            var method = type.GetMethod("MapEndpoints",
                BindingFlags.Public | BindingFlags.Static);

            method?.Invoke(null, [app]);
        }

        return app;
    }
}