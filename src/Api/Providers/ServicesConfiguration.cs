using QaCoders_Net.Interfaces.Services;
using QaCoders_Net.Services;

namespace QaCoders_Net.Providers;

public static class ServicesConfiguration
{
    public static IServiceCollection AddServies(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
