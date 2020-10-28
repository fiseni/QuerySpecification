﻿using FluentAssertions;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PozitronDev.QuerySpecification.UnitTests
{
    public class SpecificationBuilderExtensions_InMemory
    {
        [Fact]
        public void SetsNothing_GivenNoInMemoryExpression()
        {
            var spec = new StoreEmptySpec();

            spec.InMemory.Should().BeNull();
        }

        [Fact]
        public void SetsNothing_GivenSelectorSpecWithNoInMemoryExpression()
        {
            var spec = new StoreNamesEmptySpec();

            spec.InMemory.Should().BeNull();
        }

        [Fact]
        public void SetsInMemoryPredicate_GivenInMemoryExpression()
        {
            var spec = new StoreWithInMemorySpec();

            spec.InMemory.Should().NotBeNull();
        }

        [Fact]
        public void SetsInMemoryPredicate_GivenSelectorSpecWithInMemoryExpression()
        {
            var spec = new StoreNamesWithInMemorySpec();

            spec.InMemory.Should().NotBeNull();
        }
    }
}
