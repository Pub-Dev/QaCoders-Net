using NUnit.Framework;

public class BaseTest
{
    [SetUp]
    public void Setup()
    {
        TestContext.Progress.WriteLine($"Starting Test - {TestContext.CurrentContext.Test.Name}");
    }

    [TearDown]
    public void TearDown()
    {
        TestContext.Progress.WriteLine($"Finishing Test - {TestContext.CurrentContext.Test.Name}");
    }
}