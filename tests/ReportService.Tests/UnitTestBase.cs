using AutoFixture;

namespace ReportService.Tests;

public abstract class UnitTestBase<TObjectUnderTest> where TObjectUnderTest : class
{
    protected Fixture Fixture { get; set; } = null!;
    protected TObjectUnderTest Instance { get; set; } = null!;
    
    [OneTimeSetUp]
    public virtual void OneTimeSetUp()
    {
        Fixture = new Fixture();
    }

    [SetUp]
    public abstract void Setup();
}