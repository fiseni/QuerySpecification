<img align="left" src="pozitronlogo.png" width="120" height="120">

&nbsp; [![Build Status](https://dev.azure.com/pozitrondev/PozitronDev.QuerySpecification/_apis/build/status/fiseni.PozitronDev.QuerySpecification?branchName=master)](https://dev.azure.com/pozitrondev/PozitronDev.QuerySpecification/_build/latest?definitionId=4&branchName=master)

&nbsp; [![NuGet](https://img.shields.io/nuget/v/PozitronDev.QuerySpecification.svg)](https://www.nuget.org/packages/PozitronDev.QuerySpecification)[![NuGet](https://img.shields.io/nuget/dt/PozitronDev.QuerySpecification.svg)](https://www.nuget.org/packages/PozitronDev.QuerySpecification)

&nbsp; Nuget: PozitronDev.QuerySpecification

# PozitronDev QuerySpecification

Nuget package for building query specifications in your domain model. They are evaluated and utilized to create actual queries for your ORM.

This is base/abstract package intended to be utilized for various ORM implementation packages. Please check [PozitronDev.QuerySpecification.EF](https://github.com/fiseni/QuerySpecificationEF) for the EF implementation, which contains EF evaluators and generic repository ready to be consumed in your projects.

<strong>Note:</strong> The package uses the base premises of the following GitHub project [Ardalis.Specification](https://github.com/fiseni/QuerySpecificationEF). Due to the many breaking changes introduced here, and different infrastructure and usage, I decided to maintain it as a separate package for now. Feel free to check both packages and use them as you desire.

## What is specification pattern

Specification pattern in general is a way of combining and encapsulating set of business rules. Once you create them, you can test if domain objects satisfy these defined rules or not. Historically, the initiative had nothing to do with DB related queries or anything with persistence, but more with customizable business logic, which (in theory) would be easily maintainable.

I personally, throughout the years have always avoided this pattern, and considered to be an unnecessary abstraction, which does more harm than good. The composite specification classes were just a glorified If statements. Check out this example on this [Wikipedia link](https://en.wikipedia.org/wiki/Specification_pattern). It can't be more ugly than that!

### What changed

Once EF got released, we realized we can use this pattern to improve our domain model. EF utilizes/can utilize IQueryable<T> to build the queries, which in the end are parsed and materialized to particular SQL statements. Since IQueryable<T> is a BCL type, we can construct powerful abstraction within our domain model. Let's assume the following example

```
dbContext.Companies.Where(x => x.Id == 1).Where(x => x.Name == "MyCompany").ToListAsync()
```
This is a simple EF query to get set of data from DB. Now let's modify it a bit

```
dbContext.Companies.AsQueryable().Where(x => x.Id == 1).Where(x => x.Name == "MyCompany").ToListAsync()
```
This would evaluate and would give the same results as the previous one. But, if we analyze carefully the portion `Where(x => x.Id == 1).Where(x => x.Name == "MyCompany")` has nothing to do with the EF implementation, and the query is been constructed on top of IQueryable<T> (which is a BCL type). And even more importantly, that portion of code is the actual logic of the query itself.

### Why should I use it?

Most certainly this pattern is not "fit to everything" concept. It's just one way of organizing your infrastructure, especially if you're following DDD and trying to achieve one sort of clean-architecture. If you're doing microservices, then it's not for you. If you chose a vertical modularity in your solution (feature based), probably not for you either.

In terms of DDD, the focus is to create long-living domain model. It will represent code-base of your solution for years to come, on top of which you can easily attach various UI technologies, and utilize different infrastructure implementations. One way to achieve all of that, is to keep your domain model as agnostic as you can toward any infrastructural or third-party implementations. Ideally, it should be dependent purely on the .NET framework only.
There are various techniques how to achieve such an abstraction. But, in all those implementations, there is one loophole in the concept of long-living domain model. The knowledge what data is retrieved from persistence reside out of the boundaries of the model, and that's a huge barrier toward complete encapsulation of the model.

That's exactly what this package addresses. The knowledge of the "queried data" is kept within the domain model, and then just materialized through some outer implementation. We still keep our domain clean of any dependencies, and clean of any implementation details.

### What if I can't build all queries

The intention is not re-create all possible functionalities of various ORMs. Your ORM for sure will offer much more options in order to effectively build all sort of queries. You have complex queries, leave them in your persistence projects, and just keep the Interface in your domain model. That's what I do. But, surely 80% of the queries are just simple basic queries. Let's keep them in your domain model, and for the complex ones utilize the full potential of your ORM. This way, if you decide to change your ORM, you will have much less work to do.

## Usage

The usage is quite straight-forward. In this package I tried to mimic the existing LINQ approach of doing things, so I believe the usage would be quite familiar to a large audience.
Basically, you just need to inherit from the Specification abstract class. The class offers a builder "Query", and by using the builder you write your query in the constructor. That's it.
Add `PozitronDev.QuerySpecification` nuget package in your core/domain project. This package has no any dependencies other than .NET framework.

```
public class MyCompanySpec : Specification<Company>
{
    public MyCompanySpec(int id)
    {
	// It's possible to chain everything, or write them separately. 
	// It's based on your preference
	Query.Where(x => x.Id == id)
		 .Paginate(10, 20)
		 .OrderBy(x => x.Name)
			.ThenByDescending(x => x.SomeOtherCompanyInfo);

	Query.Where(x => x.Name == "MyCompany")
		 .Include(x => x.Stores)
			.ThenInclude(x => x.Addresses)

	Query.Include(x => x.Country)
    }
}
```

### Repository

In your infrastructure project, add the EF implementation nuget package `PozitronDev.QuerySpecification.EF`. Once you do that, create your own repository and inherit from the abstract repository defined in the package.

```
public class MyRepository<T> : Repository<T>
{
	private readonly MyDbContext myDbContext;

	public MyRepository(MyDbContext myDbContext)
		: base(myDbContext)
	{
		this.myDbContext = myDbContext;
	}

	// Not required to implement anything. Override or add additional functionalities.
}
```

That's it, you're ready to go. In the package there is already defined interface `IRepository<T>` which is implemented by `Repository<T>.` Just wire it in your DI container. I you're using IServiceCollection, then wire it up as following

```
services.AddScoped(typeof(IRepository<>), typeof(MyRepository<>));
```

Now, let's use and consume it from your services/controllers or whatever construct you have.

```
public class CompanyService : ICompanyService
{
	private readonly IRepository<Company> companyRepository;

	public CompanyService(IRepository<Company> companyRepository)
	{
		this.companyRepository = companyRepository;
	}
	
	public async Task<List<Company>> GetMyCompanies()
	{
		var companies = await companyRepository.ListAsync(new MyCompanySpec());

		return companies;
	}
}
```

And that's it, you got your data. I do believe it can't be any simpler.


## Give a Star! :star:
If you like or are using this project please give it a star. Thanks!
