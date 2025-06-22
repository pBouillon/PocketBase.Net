using PocketBase.Net.Client.Configuration.CollectionNamingRules;
using PocketBase.Net.Client.Entities.Records;
using Shouldly;

namespace PocketBase.Net.Client.Tests.Configuration.CollectionNamingRules;

public class SeedWithTypeNameTests
{
    public record TestRecord : RecordBase { }

    [InlineData("")]
    [InlineData("ignored")]
    [Theory(DisplayName = "Should return camelCase and ignore the current name")]
    public void BuildNameFor_ShouldIgnoreCurrentNameParameter(string name)
    {
        // Arrange
        var rule = new SeedWithTypeName();

        // Act
        var result = rule.BuildNameFor<TestRecord>(name);

        // Assert
        result.ShouldBe("testRecord");
    }
}
