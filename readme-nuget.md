A .NET library for building query specifications.
- [Pozitron.QuerySpecification](https://www.nuget.org/packages/Pozitron.QuerySpecification)
  A base package containing the core functionality, in-memory evaluators, and validators.
- [Pozitron.QuerySpecification.EntityFrameworkCore](https://www.nuget.org/packages/Pozitron.QuerySpecification.EntityFrameworkCore)
  An `EntityFramework Core` plugin to the base package. It contains EF specific evaluators.

## Getting Started

An extended list of features (in-memory collection evaluations, validations, repositories, extensions, etc.) will be soon available in the Wiki. Below are listed some basic and common usages.

### Creating and consuming specifications

Create your specifications by inheriting from the `Specification<T>` class, and use the `Query` builder in the constructor to define your conditions.

```csharp
public class CustomerSpec : Specification<Customer>
{
  public CustomerSpec(int age, string nameTerm)
  {
    Query
      .Where(x => x.Age >= age)
      .Like(x => x.Name, $"%{nameTerm}%")
      .Include(x => x.Addresses)
          .ThenInclude(x => x.Contact)
      .OrderBy(x => x.Id)
          .ThenBy(x => x.Name)
      .Skip(10)
      .Take(10)
      .AsSplitQuery();
  }
}
```

Apply the specification to `DbSet<T>` or `IQueryable<T>` source.

```csharp
var spec = new CustomerSpec(30, "Customer");

List<Customer> result = await _context
    .Customers
    .WithSpecification(spec)
    .ToListAsync();
```

### Projections

The specification can be used to project the result into a different type. Inherit from `Specification<T, TResult>` class, where TResult is the type you want to project into. This offers strongly typed experience in the builder and during the evaluation.

```csharp
public class CustomerDtoSpec : Specification<Customer, CustomerDto>
{
  public CustomerDtoSpec(int age, string nameTerm)
  {
    Query
      .Where(x => x.Age >= age)
      .Like(x => x.Name, $"%{nameTerm}%")
      .OrderBy(x => x.Name)
      .Select(x => new CustomerDto(x.Id, x.Name));
  }
}
```

Apply the specification to `DbSet<T>` or `IQueryable<T>` source.

```csharp
var spec = new CustomerDtoSpec(30, "Customer");

List<CustomerDto> result = await _context
    .Customers
    .WithSpecification(spec)
    .ToListAsync();
```

### Pagination

The library defines a convenient `ToPagedResult` extension method that returns a detailed paged result.

```csharp
var spec = new CustomerDtoSpec(1, "Customer");
var pagingFilter = new PagingFilter
{
    Page = 1,
    PageSize = 2
};

PagedResult<CustomerDto> result = await _context
    .Customers
    .WithSpecification(spec)
    .ToPagedResultAsync(pagingFilter);
```

The `PagedResult<T>` is serializable and contains a detailed pagination information and the data.

```json
{
  "Pagination": {
    "TotalItems": 100,
    "TotalPages": 50,
    "PageSize": 2,
    "Page": 1,
    "StartItem": 1,
    "EndItem": 2,
    "HasPrevious": false,
    "HasNext": true
  },
  "Data": [
    {
      "Id": 1,
      "Name": "Customer 1"
    },
    {
      "Id": 2,
      "Name": "Customer 2"
    }
  ]
}
```

## Benchmarks

In version 11, we refactored and rebuilt the internals from the ground up. The new version reduces the memory footprint drastically. The overhead of the library is now negligible and statistically insignificant. Here are the benchmark results of `ToQueryString()` for various queries. Refer to the [Benchmarks](https://github.com/fiseni/QuerySpecification/tree/main/tests/QuerySpecification.Benchmarks/Benchmarks) project for more benchmarks.

Type:
- 0 -> Empty
- 1 -> Single Where clause
- 2 -> Where and OrderBy
- 3 -> Where, Order chain, Include chain, Flag (AsNoTracking)
- 4 -> Where, Order chain, Include chain, Like, Skip, Take, Flag (AsNoTracking)

| Method | Type | Mean      | Error    | StdDev   | Ratio | Gen0    | Gen1   | Allocated | Alloc Ratio |
|------- |----- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| EFCore | 0    |  81.55 us | 0.686 us | 0.608 us |  1.00 | 10.0098 | 0.9766 |  82.54 KB |        1.00 |
| Spec   | 0    |  78.18 us | 0.472 us | 0.441 us |  0.96 | 10.0098 | 0.9766 |  82.53 KB |        1.00 |
|        |      |           |          |          |       |         |        |           |             |
| EFCore | 1    |  92.62 us | 0.350 us | 0.310 us |  1.00 | 10.2539 | 0.9766 |  84.77 KB |        1.00 |
| Spec   | 1    |  92.92 us | 0.252 us | 0.236 us |  1.00 | 10.2539 | 0.9766 |  84.84 KB |        1.00 |
|        |      |           |          |          |       |         |        |           |             |
| EFCore | 2    |  95.48 us | 0.654 us | 0.580 us |  1.00 | 10.2539 | 0.9766 |  86.03 KB |        1.00 |
| Spec   | 2    |  98.36 us | 0.775 us | 0.687 us |  1.03 | 10.2539 | 0.4883 |  86.12 KB |        1.00 |
|        |      |           |          |          |       |         |        |           |             |
| EFCore | 3    | 106.62 us | 0.684 us | 0.606 us |  1.00 | 10.7422 | 0.4883 |  90.35 KB |        1.00 |
| Spec   | 3    | 109.56 us | 0.700 us | 0.655 us |  1.03 | 10.7422 | 0.4883 |  90.64 KB |        1.00 |
|        |      |           |          |          |       |         |        |           |             |
| EFCore | 4    | 147.47 us | 0.619 us | 0.483 us |  1.00 | 13.1836 | 0.9766 | 110.78 KB |        1.00 |
| Spec   | 4    | 150.82 us | 0.538 us | 0.449 us |  1.02 | 13.1836 | 0.9766 | 111.32 KB |        1.00 |

## Give a Star! :star:
If you like or are using this project please give it a star. Thanks!
