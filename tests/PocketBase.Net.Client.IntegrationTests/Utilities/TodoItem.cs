using Bogus;
using PocketBase.Net.Client.Entities.Records;
using Shouldly;

namespace PocketBase.Net.Client.IntegrationTests.Utilities;

internal record TodoItem
{
    public string Description { get; init; } = null!;

    public bool IsCompleted { get; init; }

    public static TodoItem GenerateCompletedTodoItem()
        => new TodoItemFaker().Generate() with { IsCompleted = true };

    public static TodoItem GenerateInvalidTodoItem()
        => new TodoItemFaker().Generate() with { Description = string.Empty };

    public static TodoItem GeneratePendingTodoItem()
        => new TodoItemFaker().Generate() with { IsCompleted = false };
}

file class TodoItemFaker : Faker<TodoItem>
{
    public TodoItemFaker()
    {
        RuleFor(
            todoItem => todoItem.Description,
            faker => faker.Lorem.Sentence());
    }
}

internal record TodoItemRecord : RecordBase
{
    public string Description { get; init; } = null!;

    public bool IsCompleted { get; init; }

    public void ShouldBeEquivalentTo(TodoItemRecord other)
        => this.ShouldSatisfyAllConditions(
            // Generic properties
            (item) => item.Id.ShouldBe(other.Id),
            (item) => item.Created.ShouldBe(other.Created),
            (item) => item.Updated.ShouldBe(other.Updated),

            // Record properties
            (item) => item.Description.ShouldBe(other.Description),
            (item) => item.IsCompleted.ShouldBe(other.IsCompleted),

            // Collection-related properties
            (item) => item.CollectionId.ShouldBe(other.CollectionId),
            (item) => item.CollectionName.ShouldBe(other.CollectionName)
        );
}
