using Microsoft.Extensions.DependencyInjection;
using PocketBase.Net.Client;
using PocketBase.Net.Client.Configuration;
using PocketBase.Net.Client.Configuration.CollectionNamingRules;
using PocketBase.Net.Client.Entities.Records;
using PocketBase.Net.DependencyInjection;

var services = new ServiceCollection();

services.AddPocketBase(
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
    }).AddPocketBaseRepositories(scanningAssembly: typeof(AuthorRecord).Assembly);

var container = services.BuildServiceProvider();

var authorRepository = container.GetRequiredService<IRepository<AuthorRecord>>();

var maxBrooks = new { Name = "Max Brooks" };

var authorRecord = await authorRepository.CreateRecordFrom(maxBrooks);

var updatedAuthor = await authorRepository.UpdateRecord(
    authorRecord!.Id,
    maxBrooks with { Name = "M. Brooks" });

await authorRepository.DeleteRecord(authorRecord.Id);

// --- Definitions ---

record AuthorRecord : RecordBase
{
    public string Name { get; init; } = null!;
}
