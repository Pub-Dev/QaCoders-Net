using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QaCoders_Net.Configuration;
using System.Data;

namespace QaCoders_Net.Providers;

public static class PersistenceConfiguration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("QaCodersDatabase");

        services.AddDbContextPool<QaCodersDbContext>(options => options.UseSqlServer(connectionString));

        services.AddStartupTask<QaCodersDbContext>((service, cancelationToken) => 
            service.Database.MigrateAsync(cancelationToken));

        services.AddScoped<IDbConnection>(x => new SqlConnection(connectionString));

        return services;
    }
}
