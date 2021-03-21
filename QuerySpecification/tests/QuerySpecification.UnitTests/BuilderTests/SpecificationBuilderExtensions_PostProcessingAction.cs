using FluentAssertions;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using PozitronDev.QuerySpecification.UnitTests.Fixture.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PozitronDev.QuerySpecification.UnitTests
{
    public class SpecificationBuilderExtensions_PostProcessingAction
    {
        [Fact]
        public void SetsNothing_GivenNoPostProcessingAction()
        {
            var spec = new StoreEmptySpec();

            spec.PostProcessingAction.Should().BeNull();
        }

        [Fact]
        public void SetsNothing_GivenSelectorSpecWithNoPostProcessingAction()
        {
            var spec = new StoreNamesEmptySpec();

            spec.PostProcessingAction.Should().BeNull();
        }

        [Fact]
        public void SetsPostProcessingPredicate_GivenPostProcessingAction()
        {
            var spec = new StoreWithPostProcessingActionSpec();

            spec.PostProcessingAction.Should().NotBeNull();
        }

        [Fact]
        public void SetsPostProcessingPredicate_GivenSelectorSpecWithPostProcessingAction()
        {
            var spec = new StoreNamesWithPostProcessingActionSpec();

            spec.PostProcessingAction.Should().NotBeNull();
        }
    }
}
