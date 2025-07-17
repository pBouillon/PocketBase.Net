using PocketBase.Net.Client.Entities.Filter;
using Shouldly;

namespace PocketBase.Net.Client.Tests;

public class FilterBuilderTests
{
    [Fact(DisplayName = "Should build a filter with a nested condition")]
    public void ShouldBuildFilterWithGroup()
    {
        // Arrange + Act
        var filter = Filter.Field("id").Equal(37)
            .And().Grouped(
                Filter.Field("age").GreaterThanOrEqual(18)
                .Or().Field("role").Like("admin"))
            .And().Field("isVerified").Equal(true)
            .Build();

        // Assert
        filter.ShouldBe("id=37 && (age>=18 || role~\"admin\") && isVerified=true");
    }
    [Fact(DisplayName = "Should build a filter with a nested nested condition")]
    public void ShouldBuildFilterWithNestedGroups()
    {
        // Arrange + Act
        var filter = Filter.Field("id").Equal(37)
            .And().Grouped(
                Filter.Field("age").GreaterThanOrEqual(18)
                .Or().Grouped(
                    Filter.Field("role").Like("admin")))
            .And().Field("isVerified").Equal(true)
            .Build();

        // Assert
        filter.ShouldBe("id=37 && (age>=18 || (role~\"admin\")) && isVerified=true");
    }
}
