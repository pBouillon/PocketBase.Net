using PocketBase.Net.Client.Configuration.CollectionNamingRules;
using Shouldly;

namespace PocketBase.Net.Client.Tests.Configuration.CollectionNamingRules;

file record TestRecord { }

public class TrimSuffixTests
{
    [Fact(DisplayName = "Should trim default suffix")]
    public void BuildNameFor_ShouldTrimDefaultSuffix()
    {
        // Arrange
        var rule = new TrimSuffix();

        // Act
        var result = rule.BuildNameFor<TestRecord>("nameRecord");

        // Assert
        result.ShouldBe("name");
    }

    [Fact(DisplayName = "Should trim specified suffix")]
    public void BuildNameFor_ShouldTrimSpecifiedSuffix()
    {
        // Arrange
        var rule = new TrimSuffix("Suffix");

        // Act
        var result = rule.BuildNameFor<TestRecord>("nameSuffix");

        // Assert
        result.ShouldBe("name");
    }

    [Fact(DisplayName = "Should not trim the suffix when not found")]
    public void BuildNameFor_ShouldNotTrimSuffix_WhenNotPresent()
    {
        // Arrange
        var rule = new TrimSuffix();

        // Act
        var result = rule.BuildNameFor<TestRecord>("nonMatching");

        // Assert
        result.ShouldBe("nonMatching");
    }
}
