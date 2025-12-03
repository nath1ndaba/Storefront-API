using Storefront.API.Endpoints;

namespace Storefront.API.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapProductEndpoints();
        app.MapCartEndpoints();
        
        return app;
    }
}
