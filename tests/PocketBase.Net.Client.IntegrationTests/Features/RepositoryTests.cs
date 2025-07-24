using Microsoft.Extensions.DependencyInjection;
using PocketBase.Net.Client.Entities;
using PocketBase.Net.Client.Entities.Filter;
using PocketBase.Net.Client.Exceptions;
using PocketBase.Net.Client.IntegrationTests.Fixtures;
using PocketBase.Net.Client.IntegrationTests.Utilities;
using Shouldly;

namespace PocketBase.Net.Client.IntegrationTests.Features;

public class RepositoryTests(PocketBaseFixture fixture)
    : IClassFixture<PocketBaseFixture>
{
    [Trait("scenario", "failure")]
    [Fact(DisplayName = "Record creation should fail when the payload does not respect the constraints")]
    public async Task CreateRecordFrom_WithUnknownCollection_ShouldFail()
    {
        // Arrange
        var repository = fixture.ServiceProvider
            .GetRequiredService<IRepository<TodoItemRecord>>();

        // Act & Assert
        _ = await Should.ThrowAsync<RecordCreationFailedException>(
            () => repository.CreateRecordFrom(TodoItem.GenerateInvalidTodoItem()));
    }

    [Trait("scenario", "happy path")]
    [Fact(DisplayName = "Repository operations should behave as expected")]
    public async Task RepositoryOperations_ShouldBehaveAsExpected()
    {
        var repository = fixture.ServiceProvider
            .GetRequiredService<IRepository<TodoItemRecord>>();

        // Record creation
        var pendingTodoItem = TodoItem.GeneratePendingTodoItem();
        var pendingTodoItemEntity = await repository.CreateRecordFrom(pendingTodoItem);

        var completedTodoItem = TodoItem.GenerateCompletedTodoItem();
        var completedTodoItemEntity = await repository.CreateRecordFrom(completedTodoItem);

        pendingTodoItemEntity.ShouldSatisfyAllConditions(
            () => pendingTodoItemEntity.Description.ShouldBe(pendingTodoItem.Description),
            () => pendingTodoItemEntity.IsCompleted.ShouldBe(pendingTodoItem.IsCompleted));

        // Record fetching
        var fetchedTodoItems = await repository.GetRecords();
        fetchedTodoItems.ShouldSatisfyAllConditions(
            (todoItems) => todoItems.TotalItems.ShouldBe(2),
            (todoItems) => todoItems.PageNumber.ShouldBe(1),
            (todoItems) => todoItems.Items.Count.ShouldBe(2),
            (todoItems) => todoItems.Items[0].ShouldBeEquivalentTo(pendingTodoItemEntity),
            (todoItems) => todoItems.Items[1].ShouldBeEquivalentTo(completedTodoItemEntity));

        var fetchedTodoItem = await repository.GetRecord(pendingTodoItemEntity.Id);
        fetchedTodoItem.ShouldBeEquivalentTo(pendingTodoItemEntity);

        // Records search
        var completedTodoItemsSearchFilter = Filter
            .Field("isCompleted").Equal(true)
            .Build();

        var searchedCompletedTodoItems = await repository
            .Query()
            .WithFilter(completedTodoItemsSearchFilter)
            .ExecuteAsync();

        searchedCompletedTodoItems.ShouldSatisfyAllConditions(
            (todoItems) => todoItems.TotalItems.ShouldBe(1),
            (todoItems) => todoItems.PageNumber.ShouldBe(1),
            (todoItems) => todoItems.Items.Count.ShouldBe(1),
            (todoItems) => todoItems.Items[0].ShouldBeEquivalentTo(completedTodoItemEntity));

        // Records pagination
        var singlePagedTodoItems = await repository
            .Query()
            .WithPagination(new PaginationOptions
            {
                ItemsPerPage = 1,
                PageNumber = 2,
            })
            .ExecuteAsync();

        singlePagedTodoItems.ShouldSatisfyAllConditions(
            (todoItems) => todoItems.TotalItems.ShouldBe(2),
            (todoItems) => todoItems.PageNumber.ShouldBe(2),
            (todoItems) => todoItems.Items.Count.ShouldBe(1));

        // Pagination and search
        var singlePagedCompletedTodoItem = await repository
            .Query()
            .WithFilter(completedTodoItemsSearchFilter)
            .WithPagination(new PaginationOptions
            {
                ItemsPerPage = 1,
                PageNumber = 1,
            })
            .ExecuteAsync();

        singlePagedCompletedTodoItem.ShouldSatisfyAllConditions(
            (todoItems) => todoItems.TotalItems.ShouldBe(1),
            (todoItems) => todoItems.PageNumber.ShouldBe(1),
            (todoItems) => todoItems.Items.Count.ShouldBe(1),
            (todoItems) => todoItems.Items[0].ShouldBeEquivalentTo(completedTodoItemEntity));

        // Sorting results
        var sortByStatusDescendingThenName = Sort
            .ByDescending("isCompleted")
            .ThenBy("description")
            .Build();

        var todoItemsByStatusDescendingThenName = await repository
            .Query()
            .WithSorting(sortByStatusDescendingThenName)
            .ExecuteAsync();

        todoItemsByStatusDescendingThenName.ShouldSatisfyAllConditions(
            (items) => items.Items[0].ShouldBeEquivalentTo(completedTodoItemEntity),
            (items) => items.Items[1].ShouldBeEquivalentTo(pendingTodoItemEntity));

        var sortByStatusThenNameDescending = Sort
            .By("isCompleted")
            .ThenByDescending("description")
            .Build();

        var todoItemsByStatusThenNameDescending = await repository
            .Query()
            .WithSorting(sortByStatusThenNameDescending)
            .ExecuteAsync();

        todoItemsByStatusThenNameDescending.ShouldSatisfyAllConditions(
            (items) => items.Items[0].ShouldBeEquivalentTo(pendingTodoItemEntity),
            (items) => items.Items[1].ShouldBeEquivalentTo(completedTodoItemEntity));

        // Record modification
        var updated = await repository.UpdateRecord(
            pendingTodoItemEntity.Id,
            new { IsCompleted = true });

        updated.ShouldSatisfyAllConditions(
            () => updated.Id.ShouldBe(pendingTodoItemEntity.Id),
            () => updated.Created.ShouldBe(pendingTodoItemEntity.Created),
            () => updated.Updated.ShouldNotBe(pendingTodoItemEntity.Updated),

            () => updated.IsCompleted.ShouldBeTrue(),
            () => updated.Description.ShouldBe(pendingTodoItem.Description)
        );

        (await repository.GetRecord(pendingTodoItemEntity.Id))
            .ShouldBeEquivalentTo(updated);

        // Record deletion
        await repository.DeleteRecord(pendingTodoItemEntity.Id);
        await repository.DeleteRecord(completedTodoItemEntity.Id);

        _ = Should.ThrowAsync<RecordSearchFailedException>(() => repository.GetRecord(pendingTodoItemEntity.Id));

        fetchedTodoItems = await repository.GetRecords();
        fetchedTodoItems.ShouldSatisfyAllConditions(
            (todoItems) => todoItems.TotalItems.ShouldBe(0),
            (todoItems) => todoItems.PageNumber.ShouldBe(1),
            (todoItems) => todoItems.Items.ShouldBeEmpty());
    }
}
