<img align="left" src="pozitronlogo.png" width="120" height="120">

&nbsp; [![NuGet](https://img.shields.io/nuget/v/Pozitron.QuerySpecification.svg)](https://www.nuget.org/packages/Pozitron.QuerySpecification)

&nbsp; [![Full Build](https://github.com/fiseni/QuerySpecification/actions/workflows/build.yml/badge.svg)](https://github.com/fiseni/QuerySpecification/actions/workflows/build.yml)

&nbsp; [![Coverage](https://queryspecification.fiseni.com/badge_combined.svg)](https://queryspecification.fiseni.com)

---

A .NET library for building query specifications.
- [Pozitron.QuerySpecification](https://www.nuget.org/packages/Pozitron.QuerySpecification)
  A base package containing the core functionality, in-memory evaluators, and validators.
- [Pozitron.QuerySpecification.EntityFrameworkCore](https://www.nuget.org/packages/Pozitron.QuerySpecification.EntityFrameworkCore)
  An `EntityFramework Core` plugin to the base package. It contains EF specific evaluators.

## Usage

Create your specification classes by inheriting from the `Specification<T>` class, and use the builder `Query` to build your queries in the constructor.

```csharp
public class CustomerSpec : Specification<Customer>
{
  public CustomerSpec(int age, string nameTerm)
  {
    Query
      .Where(x => x.Age > age)
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

Apply the specification to `DbSet<T>` or any `IQueryable<T>` source.

```csharp
var spec = new CustomerSpec(30, "John");

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
      .Where(x => x.Age > age)
      .Like(x => x.Name, $"%{nameTerm}%")
      .OrderBy(x => x.Name)
      .Select(x => new CustomerDto(x.Id, x.Name));
  }
}
```

Apply the specification to `DbSet<T>` or any `IQueryable<T>` source.

```csharp
var spec = new CustomerSpec(30, "John");

List<CustomerDto> result = await _context
    .Customers
    .WithSpecification(spec)
    .ToListAsync();
```

## Give a Star! :star:
If you like or are using this project please give it a star. Thanks!
