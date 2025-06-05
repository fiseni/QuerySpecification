using System.Reflection;

namespace Tests.Internals;

public class TypeHelperTests
{
    private static readonly Assembly[] _thisAssemblyArray = [typeof(TypeHelperTests).Assembly];

    [Fact]
    public void InstanceOf_IncludesTypeWithoutAttribute()
    {
        var result = TypeHelper.GetInstancesOf<ITestType, TestDiscoveryAttribute>(_thisAssemblyArray);

        // Should be present
        result.Should().ContainSingle(x => x is NoAttributeType);

        // Default order is int.MaxValue. Should be ordered after types with lower order
        var orderTypeIndex = result.FindIndex(x => x is TestOrderTypeA);
        var noAttrIndex = result.FindIndex(x => x is NoAttributeType);
        noAttrIndex.Should().BeGreaterThan(orderTypeIndex);
    }

    [Fact]
    public void InstanceOf_ReturnsInstance_GivenParameterlessConstructor()
    {
        var result = TypeHelper.GetInstancesOf<ITestType, TestDiscoveryAttribute>(_thisAssemblyArray);

        result.Should().ContainSingle(x=>x is TestCtorType);
    }

    [Fact]
    public void InstanceOf_ReturnsInstance_GivenSingletonFromStaticField()
    {
        var result = TypeHelper.GetInstancesOf<ITestType, TestDiscoveryAttribute>(_thisAssemblyArray);

        result.Should().ContainSingle(x => x is TestFieldType && ReferenceEquals(x, TestFieldType.Instance));
    }

    [Fact]
    public void InstanceOf_ReturnsInstance_GivenSingletonFromStaticProperty()
    {
        var result = TypeHelper.GetInstancesOf<ITestType, TestDiscoveryAttribute>(_thisAssemblyArray);

        result.Should().ContainSingle(x => x is TestPropertyType && ReferenceEquals(x, TestPropertyType.Instance));
    }

    [Fact]
    public void InstanceOf_DoesNotReturnsInstance_GivenPrivateConstructorAndNoSingleton()
    {
        var result = TypeHelper.GetInstancesOf<ITestType, TestDiscoveryAttribute>(_thisAssemblyArray);

        result.Should().NotContain(x => x is TestPrivateCtorType);
    }

    [Fact]
    public void InstanceOf_SkipsTypesWithDisabledDiscovery()
    {
        var result = TypeHelper.GetInstancesOf<ITestType, TestDiscoveryAttribute>(_thisAssemblyArray);

        result.Should().NotContain(x => x is TestDisabledType);
    }

    [Fact]
    public void InstanceOf_SkipsAbstractAndNonAssignableTypes()
    {
        var result = TypeHelper.GetInstancesOf<ITestType, TestDiscoveryAttribute>(_thisAssemblyArray);

        result.Should().NotContain(x => x is TestAbstractType);
        result.Should().NotContain(x => x is NotAssignableType);
    }

    [Fact]
    public void InstanceOf_OrdersByOrderThenTypeName()
    {
        var result = TypeHelper.GetInstancesOf<ITestType, TestDiscoveryAttribute>(_thisAssemblyArray);

        var indexA = result.FindIndex(x => x is TestOrderTypeA);
        var indexB = result.FindIndex(x => x is TestOrderTypeB);
        indexA.Should().BeLessThan(indexB, "TestOrderTypeA should come before TestOrderTypeB");
    }

    // Helper types for testing
    public interface ITestType { }
    public class TestDiscoveryAttribute : DiscoveryAttribute { }

    public abstract class TestAbstractType : ITestType { }
    public class NotAssignableType { }
    public class NoAttributeType : ITestType { }

    [TestDiscovery]
    public class TestCtorType : ITestType { }

    [TestDiscovery]
    public class TestPrivateCtorType : ITestType
    {
        private TestPrivateCtorType() { }
    }

    [TestDiscovery]
    public class TestFieldType : ITestType
    {
        public static TestFieldType Instance = new();
        private TestFieldType() { }
    }

    [TestDiscovery]
    public class TestPropertyType : ITestType
    {
        public static TestPropertyType Instance { get; } = new();
        private TestPropertyType() { }
    }

    [TestDiscovery(Enable = false)]
    public class TestDisabledType : ITestType { }

    [TestDiscovery(Order = 1)]
    public class TestOrderTypeA : ITestType { }

    [TestDiscovery(Order = 2)]
    public class TestOrderTypeB : ITestType { }
}
