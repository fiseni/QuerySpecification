﻿using System.Runtime.CompilerServices;

namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class SpecificationEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly SpecificationEvaluator _evaluator = SpecificationEvaluator.Default;

    public record Customer(int Id, string FirstName, string LastName, List<string>? Emails = null);

    [Fact]
    public void ThrowsArgumentNullException_GivenNullSpec()
    {
        var sut = () => _evaluator.Evaluate(DbContext.Countries, (Specification<Country>)null!);

        sut.Should().Throw<ArgumentNullException>().WithParameterName("specification");
    }

    [Fact]
    public void ThrowsArgumentNullException_GivenNullSpecificationWithSelector()
    {
        var sut = () => _evaluator.Evaluate(DbContext.Countries, (Specification<Country, string>)null!);

        sut.Should().Throw<ArgumentNullException>().WithParameterName("specification");
    }

    [Fact]
    public void ThrowsSelectorNotFoundException_GivenNoSelector()
    {
        var spec = new Specification<Country, string>();

        var sut = () => _evaluator.Evaluate(DbContext.Countries, spec);

        sut.Should().Throw<SelectorNotFoundException>();
    }

    [Fact]
    public void GivenEmptySpec()
    {
        var spec = new Specification<Store>();

        var actual = _evaluator.Evaluate(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenFullQuery()
    {
        var id = 2;
        var name = "Store1";
        var storeTerm = "ab1";
        var companyTerm = "ab2";
        var streetTerm = "ab3";

        var spec = new Specification<Store>();
        spec.Query
            .TagWith("full test query")
            .TagWith("another tag")
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Like(x => x.Name, $"%{storeTerm}%")
            .Like(x => x.Company.Name, $"%{companyTerm}%")
            .Like(x => x.Address.Street, $"%{streetTerm}%", 2)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters();

        var actual = _evaluator.Evaluate(DbContext.Stores, spec)
            .ToQueryString();

        // The expression in the spec are applied in a predefined order.
        var expected = DbContext.Stores
            .TagWith("full test query")
            .TagWith("another tag")
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                     || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .IgnoreQueryFilters()
            // Pagination always applied in the end
            .Skip(1)
            .Take(10)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenExpressionsInRandomOrder()
    {
        var id = 2;
        var name = "Store1";
        var storeTerm = "ab1";
        var companyTerm = "ab2";
        var streetTerm = "ab3";

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > id)
            .Like(x => x.Name, $"%{storeTerm}%")
            .Like(x => x.Company.Name, $"%{companyTerm}%")
            .Where(x => x.Name == name)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .Like(x => x.Address.Street, $"%{streetTerm}%", 2)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters();

        var actual = _evaluator.Evaluate(DbContext.Stores, spec)
            .ToQueryString();

        // The expression in the spec are applied in a predefined order.
        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                     || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .IgnoreQueryFilters()
            // Pagination always applied in the end
            .Skip(1)
            .Take(10)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenFullQueryWithSelect()
    {
        var id = 2;
        var name = "Store1";
        var storeTerm = "ab1";
        var companyTerm = "ab2";
        var streetTerm = "ab3";

        var spec = new Specification<Store, string?>();
        spec.Query
            .TagWith("full test query with select")
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Like(x => x.Name, $"%{storeTerm}%")
            .Like(x => x.Company.Name, $"%{companyTerm}%")
            .Like(x => x.Address.Street, $"%{streetTerm}%", 2)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters()
            .Select(x => x.Name);

        var actual = _evaluator.Evaluate(DbContext.Stores, spec)
            .ToQueryString();

        // The expression in the spec are applied in a predefined order.
        var expected = DbContext.Stores
            .TagWith("full test query with select")
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .IgnoreQueryFilters()
            .Select(x => x.Name)
            // Pagination always applied in the end
            .Skip(1)
            .Take(10)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenFullQueryWithSelectMany()
    {
        var id = 2;
        var name = "Store1";
        var storeTerm = "ab1";
        var companyTerm = "ab2";
        var streetTerm = "ab3";

        var spec = new Specification<Store, string?>();
        spec.Query
            .TagWith("full test query with select many")
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Like(x => x.Name, $"%{storeTerm}%")
            .Like(x => x.Company.Name, $"%{companyTerm}%")
            .Like(x => x.Address.Street, $"%{streetTerm}%", 2)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters()
            .SelectMany(x => x.Products.Select(x => x.Name));

        var actual = _evaluator.Evaluate(DbContext.Stores, spec)
            .ToQueryString();

        // The expression in the spec are applied in a predefined order.
        var expected = DbContext.Stores
            .TagWith("full test query with select many")
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .IgnoreQueryFilters()
            .SelectMany(x => x.Products.Select(x => x.Name))
            // Pagination always applied in the end
            .Skip(1)
            .Take(10)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenSpecAndIgnorePagination()
    {
        var id = 2;

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > id)
            .Skip(1)
            .Take(10);

        var actual = _evaluator.Evaluate(DbContext.Stores, spec, true)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenSpecWithSelectAndIgnorePagination()
    {
        var id = 2;

        var spec = new Specification<Store, string?>();
        spec.Query
            .Where(x => x.Id > id)
            .Skip(1)
            .Take(10)
            .Select(x => x.Name);

        var actual = _evaluator.Evaluate(DbContext.Stores, spec, true)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .Select(x => x.Name)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenSpecWithSelectManyAndIgnorePagination()
    {
        var id = 2;

        var spec = new Specification<Store, string?>();
        spec.Query
            .Where(x => x.Id > id)
            .Skip(1)
            .Take(10)
            .SelectMany(x => x.Products.Select(x => x.Name));

        var actual = _evaluator.Evaluate(DbContext.Stores, spec, true)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .SelectMany(x => x.Products.Select(x => x.Name))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenSpecWithMultipleFlags()
    {
        var spec = new Specification<Country>();
        spec.Query
            .IgnoreQueryFilters()
            .AsTracking()
            .AsNoTrackingWithIdentityResolution()
            .AsNoTracking() // the last one overwrites other Tracking behavior.
            .AsSplitQuery()
            .IgnoreAutoIncludes();

        var actual = _evaluator.Evaluate(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .IgnoreAutoIncludes()
            .IgnoreQueryFilters()
            .AsSplitQuery()
            .AsNoTracking()
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void Constructor_SetsDefaultEvaluators()
    {
        var expectedEvaluators = new List<IEvaluator>
        {
            WhereEvaluator.Instance,
            LikeEvaluator.Instance,
            IncludeStringEvaluator.Instance,
            IncludeEvaluator.Instance,
            OrderEvaluator.Instance,
            QueryTagEvaluator.Instance,
            IgnoreAutoIncludesEvaluator.Instance,
            IgnoreQueryFiltersEvaluator.Instance,
            AsSplitQueryEvaluator.Instance,
            AsNoTrackingEvaluator.Instance,
            AsNoTrackingWithIdentityResolutionEvaluator.Instance,
            AsTrackingEvaluator.Instance,
        };

        var evaluator = new SpecificationEvaluator();

        var result = EvaluatorsOf(evaluator);
        result.Should().HaveSameCount(expectedEvaluators);
        result.Should().Equal(expectedEvaluators);
    }

    [Fact]
    public void Constructor_SetsProvidedEvaluators()
    {
        var evaluators = new List<IEvaluator>
        {
            WhereEvaluator.Instance,
            OrderEvaluator.Instance,
            WhereEvaluator.Instance,
        };

        var evaluator = new SpecificationEvaluator(evaluators);

        var result = EvaluatorsOf(evaluator);
        result.Should().HaveSameCount(evaluators);
        result.Should().Equal(evaluators);
    }

    [Fact]
    public void DerivedSpecificationEvaluatorCanAlterDefaultEvaluators()
    {
        var expectedEvaluators = new List<IEvaluator>
        {
            LikeEvaluator.Instance,
            WhereEvaluator.Instance,
            LikeEvaluator.Instance,
            IncludeStringEvaluator.Instance,
            IncludeEvaluator.Instance,
            OrderEvaluator.Instance,
            QueryTagEvaluator.Instance,
            IgnoreAutoIncludesEvaluator.Instance,
            IgnoreQueryFiltersEvaluator.Instance,
            AsSplitQueryEvaluator.Instance,
            AsNoTrackingEvaluator.Instance,
            AsNoTrackingWithIdentityResolutionEvaluator.Instance,
            AsTrackingEvaluator.Instance,
            WhereEvaluator.Instance
        };

        var evaluator = new SpecificationEvaluatorDerived();

        var result = EvaluatorsOf(evaluator);
        result.Should().Equal(expectedEvaluators);
    }

    private class SpecificationEvaluatorDerived : SpecificationEvaluator
    {
        public SpecificationEvaluatorDerived()
        {
            Evaluators.Add(WhereEvaluator.Instance);
            Evaluators.Insert(0, LikeEvaluator.Instance);
        }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "<Evaluators>k__BackingField")]
    public static extern ref List<IEvaluator> EvaluatorsOf(SpecificationEvaluator @this);
}
