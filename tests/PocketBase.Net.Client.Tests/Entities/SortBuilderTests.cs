using PocketBase.Net.Client.Entities;
using Shouldly;

namespace PocketBase.Net.Client.Tests.Entities;

public class SortBuilderTests
{
    [Fact(DisplayName = "Build should return correct sort string for multiple fields")]
    public void Build_ShouldReturnCorrectSortStringForMultipleFields()
    {
        // Arrange
        var sort = Sort.ByDescending("name")
            .ThenBy("age")
            .ThenByDescending("date");

        // Act & Assert
        sort.Build().ShouldBe("-name,age,-date");
    }
}
