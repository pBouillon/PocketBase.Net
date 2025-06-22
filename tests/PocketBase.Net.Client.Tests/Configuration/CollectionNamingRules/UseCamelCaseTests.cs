using PocketBase.Net.Client.Configuration.CollectionNamingRules;
using Shouldly;

namespace PocketBase.Net.Client.Tests.Configuration.CollectionNamingRules;

file record TestRecord { }

public class UseCamelCaseTests
{
    [Fact(DisplayName = "Should transform the name to camelCase")]
    public void BuildNameFor_ShouldTransformToCamelCase()
    {
        // Arrange
        var rule = new UseCamelCase();

        // Act
        var result = rule.BuildNameFor<TestRecord>("CamelCase");

        // Assert
        result.ShouldBe("camelCase");
    }
}
