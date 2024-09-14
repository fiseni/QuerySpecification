using Xunit.Abstractions;

namespace Pozitron.QuerySpecification.EntityFrameworkCore.Tests;

[Collection("SharedCollection")]
public class SampleTests(TestFactory factory, ITestOutputHelper output) : IntegrationTest(factory)
{
    [Fact]
    public async Task Test1()
    {
        GlobalFlag.Flag++;

        output.WriteLine($"Start: {DateTime.Now:ss.FFFFFFF}, Flag: {GlobalFlag.Flag}");

        var count = await DbContext.Countries.CountAsync();
        output.WriteLine($"Country count: {count}, Date: {DateTime.Now:ss.FFFFFFF}, Flag: {GlobalFlag.Flag}");

        var country = new Country { Name = "US" };
        await SeedAsync(country);

        count = await DbContext.Countries.CountAsync();
        output.WriteLine($"Country count: {count}, Date: {DateTime.Now:ss.FFFFFFF}, Flag: {GlobalFlag.Flag}");
    }

    [Fact]
    public async Task Test2()
    {
        GlobalFlag.Flag++;

        output.WriteLine($"Start: {DateTime.Now:ss.FFFFFFF}, Flag: {GlobalFlag.Flag}");

        var count = await DbContext.Countries.CountAsync();
        output.WriteLine($"Country count: {count}, Date: {DateTime.Now:ss.FFFFFFF}, Flag: {GlobalFlag.Flag}");

        var country = new Country { Name = "US" };
        await SeedAsync(country);

        count = await DbContext.Countries.CountAsync();
        output.WriteLine($"Country count: {count}, Date: {DateTime.Now:ss.FFFFFFF}, Flag: {GlobalFlag.Flag}");
    }
}

public class GlobalFlag
{
    public static int Flag = 0;
}
