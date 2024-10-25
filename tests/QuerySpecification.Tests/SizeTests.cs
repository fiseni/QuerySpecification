using ManagedObjectSize;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace Tests;

public class SizeTests
{
    private readonly ITestOutputHelper _output;

    public SizeTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private record Customer(int Id, string? Name);

    [Fact]
    public void SpecSizes()
    {
        var listEmpty = new List<object>();
        PrintObjectSize(listEmpty);

        var listTwo = new List<object>(2);
        PrintObjectSize(listTwo);

        var listSeven = new List<object>(7);
        PrintObjectSize(listSeven);

        var arraySeven = new object[7];
        PrintObjectSize(arraySeven);

        var arrayWithTheList = new object[7];
        arrayWithTheList[0] = new List<Customer>(2);
        PrintObjectSize(arrayWithTheList);

        var specEmpty = new Specification<Customer>();
        PrintObjectSize(specEmpty);

        var whereExpression = new WhereExpression<Customer>(x => x.Id > 3);
        PrintObjectSize(whereExpression);

        var specWhere = new Specification<Customer>();

        //specWhere.AddToArray(StateType.Where, whereExpression);
        //specWhere.AddToArray(StateType.Where, whereExpression);
        //specWhere.AddToArray(StateType.Where, whereExpression);


        //specWhere.Add(whereExpression);
        //specWhere.Add(whereExpression);
        //specWhere.Query
        //    .Where(x => x.Id > 3)
        //    .OrderBy(x => x.Id > 3)
        //    .Take(2)
        //    .Skip(4)
        //    .AsSplitQuery()
        //    .AsNoTracking()
        //    .Select(x => x.Name);

        PrintObjectSize(specWhere);
    }


    private void PrintObjectSize(object obj, [CallerArgumentExpression(nameof(obj))] string caller = "")
    {
        _output.WriteLine("");
        _output.WriteLine(caller);
        _output.WriteLine($"Inclusive: {ObjectSize.GetObjectInclusiveSize(obj):N0}");
        _output.WriteLine($"Exclusive: {ObjectSize.GetObjectExclusiveSize(obj):N0}");
    }
}
