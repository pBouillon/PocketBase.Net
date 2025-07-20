using NSubstitute;
using PocketBase.Net.Client.Entities;
using PocketBase.Net.Client.Entities.Records;
using PocketBase.Net.Client.Entities.Repository;
using Shouldly;

namespace PocketBase.Net.Client.Tests.Entities.Repository;

public class QueryBuilderTests
{
    public record FamousAuthorRecord : RecordBase { }

    [Fact(DisplayName = "WithFilter should correctly set the filter on the query")]
    public void WithFilter_ShouldSetFilterOnQuery()
    {
        // Arrange
        var repository = Substitute.For<IRepository<FamousAuthorRecord>>();
        var filter = "id=37";

        // Act
        var query = new QueryBuilder<FamousAuthorRecord>(repository)
            .WithFilter(filter)
            .Build();

        // Assert
        query.Filter.ShouldBe(filter);
    }

    [Fact(DisplayName = "WithPagination should correctly set the pagination on the query")]
    public void WithPagination_ShouldSetPaginationOnQuery()
    {
        // Arrange
        var repository = Substitute.For<IRepository<FamousAuthorRecord>>();
        var pagination = new PaginationOptions
        {
            ItemsPerPage = 13,
            PageNumber = 37,
        };

        // Act
        var query = new QueryBuilder<FamousAuthorRecord>(repository)
            .WithPagination(pagination)
            .Build();

        // Assert
        query.PaginationOptions.ShouldBeEquivalentTo(pagination);
    }


    [Fact(DisplayName = "Build should create a query with default values if not set")]
    public void Build_ShouldCreateDefaultQuery()
    {
        // Arrange
        var repository = Substitute.For<IRepository<FamousAuthorRecord>>();

        // Act
        var query = new QueryBuilder<FamousAuthorRecord>(repository).Build();

        // Assert
        query.Filter.ShouldBeEmpty();
        query.PaginationOptions.ShouldBeEquivalentTo(PaginationOptions.Default);
    }
}
