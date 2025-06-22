using PocketBase.Net.Client.Configuration.CollectionNamingRules;
using PocketBase.Net.Client.Entities.Records;
using Shouldly;

namespace PocketBase.Net.Client.Tests.Configuration.CollectionNamingRules;

file record TestRecord : RecordBase { }

file record OtherRecord : RecordBase { }

file record SubclassRecord : TestRecord { }

public class ForRecordTests
{
    [Fact(DisplayName = "Should apply transformation")]
    public void BuildNameFor_ShouldApplyTransformation_WhenTypesMatch()
    {
        // Arrange
        var rule = new ForRecord<TestRecord>(name => $"{name}_transformed");

        // Act
        var result = rule.BuildNameFor<TestRecord>("original");

        // Assert
        result.ShouldBe("original_transformed");
    }

    [Fact(DisplayName = "Should not apply transformation when types differ")]
    public void BuildNameFor_ShouldNotApplyTransformation_WhenTypesDiffer()
    {
        // Arrange
        var rule = new ForRecord<TestRecord>(name => $"{name}_transformed");

        // Act
        var result = rule.BuildNameFor<OtherRecord>("original");

        // Assert
        result.ShouldBe("original");
    }

    [Fact(DisplayName = "Should not apply transformation whith subclass of the targeted type")]
    public void BuildNameFor_ShouldNotApplyTransformation_WithSubclassOfTargetedType()
    {
        // Arrange
        var rule = new ForRecord<TestRecord>(name => $"{name}_transformed");

        // Act
        var result = rule.BuildNameFor<SubclassRecord>("original");

        // Assert
        result.ShouldBe("original");
    }
}
