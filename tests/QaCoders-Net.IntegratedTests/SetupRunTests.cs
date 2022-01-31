using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using QaCoders_Net.Configuration;
using System;

[SetUpFixture]
public class SetupRunTests : TestHost<Startup>
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var dbContext = Scope.ServiceProvider.GetRequiredService<QaCodersDbContext>();

        dbContext.Database.Migrate();

        TestContext.Progress.WriteLine($"Start Execution - {DateTime.UtcNow}");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        var dbContext = Scope.ServiceProvider.GetRequiredService<QaCodersDbContext>();

        dbContext.Database.EnsureDeleted();

        TestContext.Progress.WriteLine($"End Execution - {DateTime.UtcNow}");
    }
}
