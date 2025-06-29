using Microsoft.Extensions.DependencyInjection;
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
        var todoItem = TodoItem.GeneratePendingTodoItem();
        var created = await repository.CreateRecordFrom(todoItem);

        created.ShouldSatisfyAllConditions(
            () => created.Description.ShouldBe(todoItem.Description),
            () => created.IsCompleted.ShouldBe(todoItem.IsCompleted));

        // Record fetching
        var fetchedTodoItems = await repository.GetRecords();
        fetchedTodoItems.ShouldSatisfyAllConditions(
            () => fetchedTodoItems.TotalItems.ShouldBe(1),
            () => fetchedTodoItems.PageOffset.ShouldBe(1),
            () => fetchedTodoItems.Items.Count.ShouldBe(1),
            () => fetchedTodoItems.Items.First().ShouldBeEquivalentTo(created));

        var fetchedTodoItem = await repository.GetRecord(created.Id);
        fetchedTodoItem.ShouldBeEquivalentTo(created);

        // Record modification
        var updated = await repository.UpdateRecord(
            created.Id,
            new { IsCompleted = true });

        updated.ShouldSatisfyAllConditions(
            () => updated.Id.ShouldBe(created.Id),
            () => updated.Created.ShouldBe(created.Created),
            () => updated.Updated.ShouldNotBe(created.Updated),

            () => updated.IsCompleted.ShouldBeTrue(),
            () => updated.Description.ShouldBe(todoItem.Description)
        );

        (await repository.GetRecord(created.Id))
            .ShouldBeEquivalentTo(updated);

        // Record deletion
        await repository.DeleteRecord(created.Id);
        _ = Should.ThrowAsync<RecordSearchFailedException>(() => repository.GetRecord(created.Id));

        fetchedTodoItems = await repository.GetRecords();
        fetchedTodoItems.ShouldSatisfyAllConditions(
            () => fetchedTodoItems.TotalItems.ShouldBe(0),
            () => fetchedTodoItems.PageOffset.ShouldBe(1),
            () => fetchedTodoItems.Items.ShouldBeEmpty());
    }
}
