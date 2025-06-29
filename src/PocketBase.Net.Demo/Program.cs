using Microsoft.Extensions.DependencyInjection;
using PocketBase.Net.Client;
using PocketBase.Net.Client.Configuration;
using PocketBase.Net.Client.Configuration.CollectionNamingRules;
using PocketBase.Net.Client.Entities.Records;
using PocketBase.Net.DependencyInjection;

// Register services and configuration
var container = new ServiceCollection()
    .AddPocketBase(
        serverUrl: new Uri("http://localhost:8090"),
        credentials: new PocketBaseClientCredentials
        {
            Identity = "technical@account.com",
            Password = "PleaseDontHackMe",
            CollectionName = "_superusers",
        },
        (pocketBaseConfiguration) =>
        {
            pocketBaseConfiguration.RecordOperationBehavior = RecordOperationBehavior.Strict;

            pocketBaseConfiguration.CollectionNamingPipeline.AppendRule(
                new ForRecord<AuthorRecord>((_) => "authors")
            );
        })
    .AddPocketBaseRepositories(scanningAssembly: typeof(AuthorRecord).Assembly)
    .BuildServiceProvider();

// Using a repository
var authorRepository = container.GetRequiredService<IRepository<AuthorRecord>>();

// Creating records
var maxBrooks = new { Name = "Max Brooks" };
var authorRecord = await authorRepository.CreateRecordFrom(maxBrooks);

await authorRepository.CreateRecordFrom(new { Name = "David Foenkinos" });

// Updating a record
await authorRepository.UpdateRecord(
    authorRecord!.Id,
    maxBrooks with { Name = "M. Brooks" });

// Retrieving a record
var fetchedAuthor = await authorRepository.GetRecord(authorRecord.Id);

// Retrieving many records
var authorRecords = await authorRepository.GetRecords();

// Delete an item
authorRecords.Items.ForEach(async item => await authorRepository.DeleteRecord(item.Id));

// --- Definitions ---

record AuthorRecord : RecordBase
{
    public string Name { get; init; } = null!;
}
