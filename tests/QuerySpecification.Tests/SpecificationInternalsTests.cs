using System.Runtime.CompilerServices;

namespace Tests;

public class SpecificationInternalsTests
{
    private static readonly SpecItem _emptySpecItem = new();

    public record Customer(int Id, string Name, Address Address);
    public record Address(int Id, City City);
    public record City(int Id, string Name);

    [Fact]
    public void AddInternal_InitializesArray_GivenFirstAddition()
    {
        var spec = new Specification<Customer>();
        var item = new SpecItem
        {
            Type = -1,
            Bag = 1,
            Reference = new object(),
        };

        spec.AddInternal(item.Type, item.Reference, item.Bag);

        var items = Accessors<Customer>.Items(spec);
        items.Should().NotBeNull();
        items.Should().HaveCount(2);
        items![0].Should().Be(item);
        items[1].Should().Be(_emptySpecItem);
    }

    [Fact]
    public void AddInternal_AddsInAvailableSlot()
    {
        var spec = new Specification<Customer>();
        var item = new SpecItem
        {
            Type = -1,
            Bag = 1,
            Reference = new object(),
        };

        spec.AddInternal(item.Type, item.Reference, item.Bag);
        spec.AddInternal(item.Type, item.Reference, item.Bag);

        var items = Accessors<Customer>.Items(spec);
        items.Should().NotBeNull();
        items.Should().HaveCount(2);
        items![0].Should().Be(item);
        items[1].Should().Be(item);
    }

    [Fact]
    public void AddInternal_ResizesArrayByFour_GivenFullCapacity()
    {
        var spec = new Specification<Customer>();
        var item = new SpecItem
        {
            Type = -1,
            Bag = 1,
            Reference = new object(),
        };

        spec.AddInternal(item.Type, item.Reference, item.Bag);
        spec.AddInternal(item.Type, item.Reference, item.Bag);
        spec.AddInternal(item.Type, item.Reference, item.Bag);

        var items = Accessors<Customer>.Items(spec);
        items.Should().NotBeNull();
        items.Should().HaveCount(6);
        items![0].Should().Be(item);
        items[1].Should().Be(item);
        items[2].Should().Be(item);
        items[3].Should().Be(_emptySpecItem);
        items[4].Should().Be(_emptySpecItem);
        items[5].Should().Be(_emptySpecItem);
    }

    [Fact]
    public void AddInternal_AddsPagingAndFlagsInSameSlot()
    {
        var spec = new Specification<Customer>();
        spec.Query.AsNoTracking();
        var itemPaging = new SpecItem
        {
            Type = ItemType.Paging,
            Bag = int.MaxValue,
            Reference = new SpecPaging(),
        };

        spec.AddInternal(itemPaging.Type, itemPaging.Reference, itemPaging.Bag);

        var items = Accessors<Customer>.Items(spec);
        items.Should().NotBeNull();
        items.Should().HaveCount(2);
        items![0].Type.Should().Be(ItemType.Paging);
        items[0].Type.Should().Be(ItemType.Flags);
        items[0].Reference.Should().Be(itemPaging.Reference);
        items[0].Bag.Should().NotBe(itemPaging.Bag);
        items[0].Bag.Should().Be((int)SpecFlags.AsNoTracking);
        items[1].Should().Be(_emptySpecItem);
    }

    [Fact]
    public void GetCompiledItems_ReturnsEmptyArray_GivenEmptySpec()
    {
        var spec = new Specification<Customer>();

        var items = spec.GetCompiledItems();

        items.Should().BeSameAs(Array.Empty<SpecItem>());
    }

    [Fact]
    public void GetCompiledItems_ReturnsEmptyArray_GivenNoCompilableItems()
    {
        var spec = new Specification<Customer>();
        spec.Query
            .Include(nameof(Customer.Address))
            .Include(x => x.Address)
            .Take(10)
            .Skip(10)
            .AsNoTracking()
            .AsSplitQuery();

        var items = spec.GetCompiledItems();

        items.Should().BeSameAs(Array.Empty<SpecItem>());
    }

    [Fact]
    public void GetCompiledItems_DoesNotRecompile_GivenNoChanges()
    {
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id > 0)
            .Like(x => x.Name, "%abc%")
            .OrderBy(x => x.Id)
            .ThenBy(x => x.Name);

        var items1 = spec.GetCompiledItems();
        var items2 = spec.GetCompiledItems();

        items1.Should().BeSameAs(items2);
    }

    [Fact]
    public void GetCompiledItems_GeneratesCompiledItems()
    {
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id > 0)
            .Like(x => x.Name, "%a1%", 2)
            .Like(x => x.Name, "%a2%")
            .OrderBy(x => x.Id)
            .ThenBy(x => x.Name);

        var items = spec.GetCompiledItems();

        items.Should().HaveCount(5);
        items[0].Reference.Should().BeOfType<Func<Customer, bool>>();
        items[0].Type.Should().Be(ItemType.Where);

        items[1].Reference.Should().BeOfType<Func<Customer, object?>>();
        items[1].Type.Should().Be(ItemType.Order);
        items[1].Bag.Should().Be((int)OrderType.OrderBy);
        items[2].Reference.Should().BeOfType<Func<Customer, object?>>();
        items[2].Type.Should().Be(ItemType.Order);
        items[2].Bag.Should().Be((int)OrderType.ThenBy);

        // Compiled like items are placed as a last segment and ordered by group.
        items[3].Reference.Should().BeOfType<SpecLikeCompiled<Customer>>();
        items[3].Type.Should().Be(ItemType.Like);
        items[3].Bag.Should().Be(1);
        items[4].Reference.Should().BeOfType<SpecLikeCompiled<Customer>>();
        items[4].Type.Should().Be(ItemType.Like);
        items[4].Bag.Should().Be(2);
    }

    [Fact]
    public void AddOrUpdateFlag_AddsNewItem_GivenSingleFlagAndSetTrue()
    {
        var spec = new Specification<Customer>();
        spec.AddOrUpdateFlag(SpecFlags.AsNoTracking, true);

        spec.AsNoTracking.Should().BeTrue();
        var items = Accessors<Customer>.Items(spec);
        items.Should().HaveCount(2);
        items![0].Type.Should().Be(ItemType.Flags);
        items![1].Should().Be(_emptySpecItem);
    }

    [Fact]
    public void AddOrUpdateFlag_DoesNotAddItem_GivenSingleFlagAndSetFalse()
    {
        var spec = new Specification<Customer>();
        spec.AddOrUpdateFlag(SpecFlags.AsNoTracking, false);

        spec.AsNoTracking.Should().BeFalse();
        Accessors<Customer>.Items(spec).Should().BeNull();
    }

    [Fact]
    public void AddOrUpdateFlag_AddsNewItem_GivenFlagAndSetTrue()
    {
        var spec = new Specification<Customer>();
        spec.Query.Where(x => x.Id > 0);
        spec.AddOrUpdateFlag(SpecFlags.AsNoTracking, true);

        spec.AsNoTracking.Should().BeTrue();
        var items = Accessors<Customer>.Items(spec);
        items.Should().HaveCount(2);
        items![0].Type.Should().Be(ItemType.Where);
        items![1].Type.Should().Be(ItemType.Flags);
    }

    [Fact]
    public void AddOrUpdateFlag_DoesNotAddItem_GivenFlagAndSetFalse()
    {
        var spec = new Specification<Customer>();
        spec.Query.Where(x => x.Id > 0);
        spec.AddOrUpdateFlag(SpecFlags.AsNoTracking, false);

        spec.AsNoTracking.Should().BeFalse();
        var items = Accessors<Customer>.Items(spec);
        items.Should().HaveCount(2);
        items![0].Type.Should().Be(ItemType.Where);
        items![1].Should().Be(_emptySpecItem);
    }

    [Fact]
    public void AddOrUpdateFlag_UpdatesFlags_GivenFlagAndSetTrue()
    {
        var spec = new Specification<Customer>();
        spec.Query.Where(x => x.Id > 0).AsSplitQuery();
        spec.AddOrUpdateFlag(SpecFlags.AsNoTracking, true);

        spec.AsNoTracking.Should().BeTrue();
        var items = Accessors<Customer>.Items(spec);
        items.Should().HaveCount(2);
        items![0].Type.Should().Be(ItemType.Where);
        items![1].Type.Should().Be(ItemType.Flags);
    }

    [Fact]
    public void AddOrUpdateFlag_UpdatesFlags_GivenFlagAndSetFalse()
    {
        var spec = new Specification<Customer>();
        spec.Query.Where(x => x.Id > 0).AsSplitQuery();
        spec.AddOrUpdateFlag(SpecFlags.AsNoTracking, false);

        spec.AsNoTracking.Should().BeFalse();
        var items = Accessors<Customer>.Items(spec);
        items.Should().HaveCount(2);
        items![0].Type.Should().Be(ItemType.Where);
        items![1].Type.Should().Be(ItemType.Flags);
    }

    [Fact]
    public void SpecFlags_ContainItemsWithPowerOfTwo()
    {
        var flags = Enum.GetValues<SpecFlags>();
        foreach (var flag in flags)
        {
            var value = (int)flag;
            value.Should().Match(num => (num > 0) && ((num & (num - 1)) == 0), "The flag value should be a power of 2");
        }
    }

    [Fact]
    public void Constructor_InitializesArray_GivenInitialCapacity()
    {
        var spec = new Specification<Customer>(10);
        Accessors<Customer>.Items(spec).Should().NotBeNull();
        Accessors<Customer>.Items(spec).Should().HaveCount(10);

        var specWithSelect = new Specification<Customer, int>(10);
        Accessors<Customer>.Items(specWithSelect).Should().NotBeNull();
        Accessors<Customer>.Items(specWithSelect).Should().HaveCount(10);
    }

    [Fact]
    public void ArrayIsNull_GivenEmptySpec()
    {
        var spec = new Specification<Customer>();

        Accessors<Customer>.Items(spec).Should().BeNull();
    }

    private class Accessors<T>
    {
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_items")]
        public static extern ref SpecItem[]? Items(Specification<T> @this);
    }
}
