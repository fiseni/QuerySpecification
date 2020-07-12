using PozitronDev.QuerySpecification.Builder;
using PozitronDev.QuerySpecification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PozitronDev.QuerySpecification.UnitTests
{
    public class IncludeAggregatorTests
    {
        [Fact]
        public void IncludeAggregator_AddPropertyNameInConstructor_ReturnsCorrectIncludeString()
        {
            var shouldReturnIncludeString = nameof(Company);
            var includeAggregator = new IncludeAggregator(shouldReturnIncludeString);

            Assert.Equal(shouldReturnIncludeString, includeAggregator.IncludeString);
        }

        [Fact]
        public void IncludeAggregator_AddNullInConstructor_ReturnsStringEmpty()
        {
            var shouldReturnIncludeString = string.Empty;
            var includeAggregator = new IncludeAggregator(null);

            Assert.Equal(shouldReturnIncludeString, includeAggregator.IncludeString);
        }

        [Fact]
        public void IncludeAggregator_AddStringEmptyInConstructor_ReturnsStringEmpty()
        {
            var shouldReturnIncludeString = string.Empty;
            var includeAggregator = new IncludeAggregator(null);

            Assert.Equal(shouldReturnIncludeString, includeAggregator.IncludeString);
        }

        [Fact]
        public void IncludeAggregator_AddNavigationPropertyName_ReturnsCorrectIncludeString()
        {
            var nameOfCompanyEntity = nameof(Company);
            var nameOfStoresNavigation = nameof(Company.Stores);
            var nameOfProductsNavigation = nameof(Store.Products);

            var shouldReturnIncludeString = $"{nameOfCompanyEntity}.{nameOfStoresNavigation}.{nameOfProductsNavigation}";

            var includeAggregator = new IncludeAggregator(nameOfCompanyEntity);
            includeAggregator.AddNavigationPropertyName(nameOfStoresNavigation);
            includeAggregator.AddNavigationPropertyName(nameOfProductsNavigation);

            Assert.Equal(shouldReturnIncludeString, includeAggregator.IncludeString);
        }

        [Fact]
        public void IncludeAggregator_AddNavigationPropertyNameAndNullInConstructor_ReturnsCorrectIncludeString()
        {
            var nameOfCompanyEntity = nameof(Company);
            var nameOfStoresNavigation = nameof(Company.Stores);
            var nameOfProductsNavigation = nameof(Store.Products);

            var shouldReturnIncludeString = $"{nameOfCompanyEntity}.{nameOfStoresNavigation}.{nameOfProductsNavigation}";

            var includeAggregator = new IncludeAggregator(null);
            includeAggregator.AddNavigationPropertyName(nameOfCompanyEntity);
            includeAggregator.AddNavigationPropertyName(nameOfStoresNavigation);
            includeAggregator.AddNavigationPropertyName(nameOfProductsNavigation);

            Assert.Equal(shouldReturnIncludeString, includeAggregator.IncludeString);
        }

        [Fact]
        public void IncludeAggregator_AddNavigationPropertyNameAndEmptyStringInConstructor_ReturnsCorrectIncludeString()
        {
            var nameOfCompanyEntity = nameof(Company);
            var nameOfStoresNavigation = nameof(Company.Stores);
            var nameOfProductsNavigation = nameof(Store.Products);

            var shouldReturnIncludeString = $"{nameOfCompanyEntity}.{nameOfStoresNavigation}.{nameOfProductsNavigation}";

            var includeAggregator = new IncludeAggregator(string.Empty);
            includeAggregator.AddNavigationPropertyName(nameOfCompanyEntity);
            includeAggregator.AddNavigationPropertyName(nameOfStoresNavigation);
            includeAggregator.AddNavigationPropertyName(nameOfProductsNavigation);

            Assert.Equal(shouldReturnIncludeString, includeAggregator.IncludeString);
        }
    }
}
