using QaCoders_Net.Interfaces.Repositories;
using QaCoders_Net.Repositories;

namespace QaCoders_Net.Providers;

public static class RepositoriesConfiguration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}