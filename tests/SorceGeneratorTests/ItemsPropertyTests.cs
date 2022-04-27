using FluentAssertions;
using Xunit;

namespace SorceGeneratorTests;

public class ItemsPropertyTests
{
    [Fact]
    public void Should_return_all_known_items()
    {
        TestCategoryWithTwoProps.Items.Should().HaveCount(2)
            .And.BeEquivalentTo(new[]
            {
                TestCategoryWithTwoProps.Fruits,
                TestCategoryWithTwoProps.Dairy
            });
    }

    [Fact]
    public void Should_return_empty_items()
    {
        TestCategoryNullProps.Items.Should().BeEmpty();
    }
}