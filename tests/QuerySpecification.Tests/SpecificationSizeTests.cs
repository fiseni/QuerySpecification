using ManagedObjectSize;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace Pozitron.QuerySpecification.Tests;

public class SpecificationSizeTests
{
    private readonly ITestOutputHelper _output;

    public SpecificationSizeTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Spec_Empty()
    {
        var spec = new Spec();
        PrintObjectSize(spec);

        var specWithArray = new SpecWithArray();
        PrintObjectSize(specWithArray);

        var specWithDictionary = new SpecWithDictionary();
        PrintObjectSize(specWithDictionary);
    }

    [Fact]
    public void Spec_Where()
    {
        var spec = new Spec();
        spec.Where = new List<object>(2) { new() };
        PrintObjectSize(spec);

        var specWithArray = new SpecWithArray();
        specWithArray.State = new object[1];
        specWithArray.State[0] = new List<object?>(2) { new() };
        PrintObjectSize(specWithArray);

        var specWithDictionary = new SpecWithDictionary();
        specWithDictionary.State = new Dictionary<int, object>(1);
        specWithDictionary.State[0] = new List<object?>(2) { new() };
        PrintObjectSize(specWithDictionary);
    }

    [Fact]
    public void Spec_Where_Order()
    {
        var spec = new Spec();
        spec.Where = new List<object>(2) { new() };
        spec.Order = new List<object>(2) { new() };
        PrintObjectSize(spec);

        // This is very idealized scenario.
        // Since the index represents the type here, if we need Include for example then we need to initialize array of size 4.
        // So, at some point we have to give up and create max size array of 7.
        var specWithArray = new SpecWithArray();
        specWithArray.State = new object[2];
        specWithArray.State[0] = new List<object?>(2) { new() };
        specWithArray.State[1] = new List<object?>(2) { new() };
        PrintObjectSize(specWithArray);

        var specWithDictionary = new SpecWithDictionary();
        specWithDictionary.State = new Dictionary<int, object>(2);
        specWithDictionary.State[0] = new List<object?>(2) { new() };
        specWithDictionary.State[1] = new List<object?>(2) { new() };
        PrintObjectSize(specWithDictionary);
    }

    [Fact]
    public void Spec_Where_Order_Take()
    {
        var spec = new Spec();
        spec.Where = new List<object>(2) { new() };
        spec.Order = new List<object>(2) { new() };
        spec.Take = 10;
        PrintObjectSize(spec);

        var specWithArray = new SpecWithArray();
        specWithArray.State = new object[3];
        specWithArray.State[0] = new List<object?>(2) { new() };
        specWithArray.State[1] = new List<object?>(2) { new object() };
        specWithArray.State[2] = new List<object?>(2) { new SpecFlags { Take = 10 } };
        PrintObjectSize(specWithArray);

        var specWithDictionary = new SpecWithDictionary();
        specWithDictionary.State = new Dictionary<int, object>(3);
        specWithDictionary.State[0] = new List<object?>(2) { new() };
        specWithDictionary.State[1] = new List<object?>(2) { new object() };
        specWithDictionary.State[2] = new List<object?>(2) { new SpecFlags { Take = 10 } };
        PrintObjectSize(specWithDictionary);
    }


    private void PrintObjectSize(object obj, [CallerArgumentExpression(nameof(obj))] string caller = "")
    {
        _output.WriteLine("");
        _output.WriteLine(caller);
        _output.WriteLine($"Inclusive: {ObjectSize.GetObjectInclusiveSize(obj):N0}");
        _output.WriteLine($"Exclusive: {ObjectSize.GetObjectExclusiveSize(obj):N0}");
    }
}

public class Spec
{
    // We always new up the Query (builder). It's an empty object but we're spending 24 bytes here on x64.
    public object Query { get; set; } = new();

    public List<object>? Where { get; set; } = null;
    public List<object>? Order { get; set; } = null;
    public List<object>? Like { get; set; } = null;
    public List<object>? Include { get; set; } = null;
    public List<string>? IncludeString { get; set; } = null;
    public Dictionary<string, object>? Items { get; set; } = null;

    public int Take { get; internal set; } = -1;
    public int Skip { get; internal set; } = -1;
    public bool IgnoreQueryFilters { get; internal set; } = false;
    public bool AsSplitQuery { get; internal set; } = false;
    public bool AsNoTracking { get; internal set; } = false;
    public bool AsNoTrackingWithIdentityResolution { get; internal set; } = false;

}

public class SpecWithArray
{
    // We always new up the Query (builder). It's an empty object but we're spending 24 bytes here on x64.
    public object Query { get; set; } = new();

    // We'll keep everything in an array.
    // The index of the array will represent the type of data we hold.
    // We'll group all flags into a single object. Otherwise we pay penalty of 24 bytes per object.
    // So the array size will have max length of 7
    public object[]? State { get; set; } = null;
}

public class SpecWithDictionary
{
    // We always new up the Query (builder). It's an empty object but we're spending 24 bytes here on x64.
    public object Query { get; set; } = new();

    // We'll keep everything in a dictionary.
    // The int key will represent the type of data we hold.
    // We'll group all flags into a single object. Otherwise we pay penalty of 24 bytes per object.
    // So the dictionary will have max 7 items.
    public Dictionary<int, object>? State { get; set; } = null;
}

public class SpecFlags
{
    public int Take { get; internal set; } = -1;
    public int Skip { get; internal set; } = -1;
    public bool IgnoreQueryFilters { get; internal set; } = false;
    public bool AsSplitQuery { get; internal set; } = false;
    public bool AsNoTracking { get; internal set; } = false;
    public bool AsNoTrackingWithIdentityResolution { get; internal set; } = false;
}
