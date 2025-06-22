using PocketBase.Net.Client.Configuration.CollectionNamingRules;
using PocketBase.Net.Client.Entities.Records;
using Shouldly;

namespace PocketBase.Net.Client.Tests.Configuration.CollectionNamingRules;

file record TestRecord : RecordBase { }

public class UseTransformationTests
{
    [Fact(DisplayName = "Should apply the transformation")]
    public void X()
    {
        // Arrange
        var rule = new UseTransformation((name) => $"{name}_transformed");

        // Act
        var result = rule.BuildNameFor<TestRecord>("name");

        // Assert
        result.ShouldBe("name_transformed");
    }
}
