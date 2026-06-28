using Dotnet_beginner_api.Application.Interfaces;
using Dotnet_beginner_api.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet_beginner_api.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}
