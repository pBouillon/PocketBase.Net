using PocketBase.Net.Client.Configuration.CollectionNamingRules;
using PocketBase.Net.Client.Entities.Records;
using Shouldly;

namespace PocketBase.Net.Client.Tests.Configuration.CollectionNamingRules;

file record TestRecord : RecordBase;

public class UsePluralTests
{
    [Fact(DisplayName = "Should add plural")]
    public void BuildNameFor_ShouldAddPlural()
    {
        // Arrange
        var rule = new UsePlural();

        // Act
        var result = rule.BuildNameFor<Record>("user");

        // Assert
        result.ShouldBe("users");
    }

    [Fact(DisplayName = "Should not add an 's' if already present")]
    public void BuildNameFor_ShouldNotAddPluralIfAlreadyPresent()
    {
        // Arrange
        var rule = new UsePlural();

        // Act
        var result = rule.BuildNameFor<Record>("users");

        // Assert
        result.ShouldBe("users");
    }
}
