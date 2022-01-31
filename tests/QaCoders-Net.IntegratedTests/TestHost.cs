using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

public abstract class TestHost<T> : IDisposable where T : TestStartupBase
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public readonly object _lock = new object();

    public IConfiguration Configuration { get; }

    public static IServiceScope Scope => ServiceProvider.CreateScope();

    public TestHost()
    {
        lock (_lock)
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false);

            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddSingleton(Configuration);

            var startup = Activator.CreateInstance<T>();

            startup.ConfigureServices(services, Configuration);

            ServiceProvider = services.BuildServiceProvider();
        }
    }

    public static TService GetService<TService>()
    {
        return Scope.ServiceProvider.GetRequiredService<TService>();
    }

    public virtual void Dispose()
    {
        Scope?.Dispose();
    }
}
