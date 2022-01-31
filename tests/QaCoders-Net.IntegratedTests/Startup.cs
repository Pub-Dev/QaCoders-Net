using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QaCoders_Net.Configuration;
using QaCodersNet.Client;
using System.Data;
using System.Data.SqlClient;

public class Startup : TestStartupBase
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var apiDefaultUrl = configuration.GetSection("ApiUrl").Value;

        services.AddProductClient(apiDefaultUrl);
        services.AddClientClient(apiDefaultUrl);
        services.AddOrderClient(apiDefaultUrl);

        var connectionString = configuration.GetConnectionString("QaCodersDatabase");        
        
        services.AddDbContextPool<QaCodersDbContext>(options => options.UseSqlServer(connectionString));
        
        services.AddScoped<IDbConnection>(x => new SqlConnection(connectionString));        
    }
}
