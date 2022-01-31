using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public abstract class TestStartupBase
{
    public abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}
