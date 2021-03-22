<img align="left" src="docs/pozitronlogo.png" width="120" height="120">

&nbsp; [![NuGet](https://img.shields.io/nuget/v/PozitronDev.QuerySpecification.svg)](https://www.nuget.org/packages/PozitronDev.QuerySpecification)[![NuGet](https://img.shields.io/nuget/dt/PozitronDev.QuerySpecification.svg)](https://www.nuget.org/packages/PozitronDev.QuerySpecification)

&nbsp; [![Build Status](https://dev.azure.com/pozitrondev/PozitronDev.QuerySpecification/_apis/build/status/QuerySpecification_BuildPackage?branchName=master)](https://dev.azure.com/pozitrondev/PozitronDev.QuerySpecification/_build/latest?definitionId=11&branchName=master)

&nbsp; [![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/pozitrondev/PozitronDev.QuerySpecification/11)](https://dev.azure.com/pozitrondev/PozitronDev.QuerySpecification/_build/latest?definitionId=11&branchName=master&view=codecoverage-tab)

&nbsp;

<strong>Note:</strong> This package is used mostly internally by my team. Please refer to the community edition [Ardalis.Specification](https://github.com/ardalis/Specification) by @ardalis. You can get better support, we're not so careless with changes and tend to provide a more stable version there. If you wanna live on the "edge" and have the newest features, then be my guest. But, be aware that I'll be breaking your code all the time :). Once the features are mature enough, I usually port them over there anyway, so you won't be missing anything. 


# PozitronDev.QuerySpecification

Nuget packages for building query specifications in your domain model. They are evaluated and utilized to create actual queries for your ORM.

Packages:
- <strong>PozitronDev.QuerySpecification:</strong> Base/abstract package which has dependency only on .NET, so you reference it from your core project and build your specifications.
- <strong>PozitronDev.QuerySpecification.EntityFrameworkCore:</strong> An `EntityFramework Core` (latest version) plugin to the base abstract package. It contains EF specific evaluators and generic repository ready to be consumed in your projects.
- <strong>PozitronDev.QuerySpecification.EntityFrameworkCore3:</strong> An `EntityFramework Core 3` plugin to the base abstract package. It contains EF specific evaluators and generic repository ready to be consumed in your projects.

## What is specification pattern

Specification pattern in general is a way of combining and encapsulating set of business rules. Once you create them, you can test if domain objects satisfy these defined rules or not. Historically, the initiative had nothing to do with DB related queries or anything with persistence, but more with customizable business logic, which (in theory) would be easily maintainable.

I personally, throughout the years have always avoided this pattern, and considered to be an unnecessary abstraction, which does more harm than good. The composite specification classes were just a glorified If statements. Check out this example on this [Wikipedia link](https://en.wikipedia.org/wiki/Specification_pattern). It can't be more ugly than that!

### What has changed?

Once EF got released, we realized we can use this pattern to improve our domain model. EF utilizes/can utilize IQueryable<T> to build the queries, which in the end are parsed and materialized to particular SQL statements. Since IQueryable<T> is a BCL type, we can construct powerful abstraction within our domain model. Let's assume the following example

```
dbContext.Companies.Where(x => x.CountryId == 1).Where(x => x.Name == "MyCompany").ToListAsync()
```
This is a simple EF query to get set of data from DB. Now let's modify it a bit

```
dbContext.Companies.AsQueryable().Where(x => x.CountryId == 1).Where(x => x.Name == "MyCompany").ToListAsync()
```
This would evaluate and would give the same results as the previous one. But, if we analyze carefully the portion `Where(x => x.CountryId == 1).Where(x => x.Name == "MyCompany")` has nothing to do with the EF implementation, and the query is been constructed on top of IQueryable<T> (which is a BCL type). And even more importantly, that portion of code is the actual logic of the query itself.

### Why should I use it?

Most certainly this pattern is not "fit to everything" concept. It's just one way of organizing your infrastructure, especially if you're following DDD and trying to achieve one sort of clean-architecture. If you're doing microservices, then it's not for you. If you chose a vertical modularity in your solution (feature based), probably not for you either.

In terms of DDD, the focus is to create long-living domain model. It will represent code-base of your solution for years to come, on top of which you can easily attach various UI technologies, and utilize different infrastructure implementations. One way to achieve all of that, is to keep your domain model as agnostic as you can toward any infrastructural or third-party implementations. Ideally, it should be dependent purely on the .NET framework only.
There are various techniques how to achieve such an abstraction. But, in all those implementations, there is one loophole in the concept of long-living domain model. The knowledge what data is retrieved from persistence reside out of the boundaries of the model, and that's a huge barrier toward complete encapsulation of the model.

That's exactly what this package addresses. The knowledge of the "queried data" is kept within the domain model, and then just materialized through some outer implementation. We still keep our domain clean of any dependencies, and clean of any implementation details.

### What if I can't build all queries?

The intention is not re-create all possible functionalities of various ORMs. Your ORM for sure will offer much more options in order to effectively build all sort of queries. You have complex queries, leave them in your persistence projects, and just keep the Interface in your domain model. That's what I do. But, surely 80% of the queries are just simple basic queries. Let's keep them in your domain model, and for the complex ones utilize the full potential of your ORM. This way, if you decide to change your ORM, you will have much less work to do.

## Usage

The usage is quite straight-forward. In this package I tried to mimic the existing LINQ approach of doing things, so I believe the usage would be quite familiar to a large audience.
Basically, you just need to inherit from the Specification abstract class. The class offers a builder "Query", and by using the builder you write your query in the constructor. 
Add `PozitronDev.QuerySpecification` nuget package in your core/domain project. This package has no any dependencies other than .NET framework.

```
public MyCompanySpec(int countryId)
{
    // It's possible to chain everything, or write them separately. 
    // It's based on your preference
    Query.Where(x => x.CountryId == countryId)
         .Skip(10)
	 .Take(20)
         .OrderBy(x => x.Name)
            .ThenByDescending(x => x.SomeOtherCompanyInfo);

    Query.Where(x => x.Name == "MyCompany")
         .Include(x => x.Stores)
            .ThenInclude(x => x.Addresses);
	    
    Query.Search(x => x.Address, "portion of address")

    Query.Include(x => x.Country);

    Query.InMemory(x =>
    {
    	// This is a capability for InMemory operations.
	// Once the data is materialized/retrieved from persistence, this predicate will be called to further manipulate the result.
        // Here you are not constrained to the specification builder.
        // You can use everything that .NET offers in order to manipulate the List
    });
}
```

In your infrastructure project, add the EF plugin nuget package `PozitronDev.QuerySpecification.EntityFrameworkCore3`. Once you do that, you can use an instance of SpecificationEvaluator to evaluate your specifications into IQueryable.

### Search feature

There are ocassions when we need to utilize SQL Like functionality. It offers some quasi full-text search. Not applicable everywhere, but there are cases where it's quite useful. As part of this package `Search` feature is provided while building the specifications. The evaluator will automatically translate and build expressions utilizing the `EF.Functions.Like` feature of Entity Framework.

The implementation complexity is hidden, and clean usage is provided. If you define following specification:

```
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string searchTerm)
    {
        Query.Search(x => x.Name, searchTerm)
             .Search(x => x.Address, searchTerm);
    }
}
```
The evaluator will translate this automatically into
	
```
dbContext.Customers.Where(x => EF.Functions.Like(x.Name, searchTerm) ||
                                EF.Functions.Like(x.Address, searchTerm));
```

You can also provide separate/different search terms for each property. The `Search` expressions ultimately will be evaluated into `OR` conditions while building the query. If you need `AND` condition, you can provide additional paremeter `SearchGroup` to `Search`. The usage is as following:

```
public class CustomerSpec : Specification<Customer>
{
    public CustomerSpec(string searchTerm)
    {
        Query.Search(x => x.Name, searchTerm, 1)
             .Search(x => x.Address, searchTerm, 1);
             .Search(x => x.Phone, searchTerm, 2);
    }
}
```
The evaluator will translate this into
	
```
dbContext.Customers.Where(x => EF.Functions.Like(x.Name, searchTerm) ||
                                EF.Functions.Like(x.Address, searchTerm))
		   .Where(x => EF.Functions.Like(x.Phone, searchTerm));
```

### Base Repository

The EF plugin nuget package `PozitronDev.QuerySpecification.EntityFrameworkCore3` contains an abstract `RepositoryBase` class. If you want you can use it as base class for your repository, thus leveraging from the provided set of standard generic methods. Inherit from it as following

```
public class Repository<T> : RepositoryBase<T>, IRepository<T>
{
    private readonly MyDbContext myDbContext;

    public Repository(MyDbContext myDbContext)
        : base(myDbContext)
    {
        this.myDbContext = myDbContext;
    }

    // Not required to implement anything. Add additional functionalities if required.
}
```

```
public interface IRepository<T> : IRepositoryBase<T>
{
}
```

That's it. Now, let's use and consume it from your services/controllers or whatever construct you have.

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
