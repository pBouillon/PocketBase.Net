<p align="center">
    <img
        height="150px"
        width="150px"
        src="https://github.com/user-attachments/assets/3840c148-4d15-40b2-a31a-0719040eda8d"
        alt="PocketBase.Net - PocketBase integration made simple for .NET projects" />
</p>

<div align="center">
    <p>PocketBase integration made simple for .NET projects</p>
</div>

---

> [!WARNING]
> PocketBase.Net is still under active development and the public API may change.
> If you have any suggestion, feel free to open a new [discussion thread](https://github.com/pBouillon/PocketBase.Net/discussions)!

## Quick Start

Here is a code snippet using the most common features of PocketBase.NET:

```csharp
// 1️⃣ PocketBase.Net services registration
var services = new ServiceCollection()
    .AddPocketBase(
        serverUrl: new Uri("..."),
        credentials: new PocketBaseClientCredentials
        {
            Identity = "...",
            Password = "...",
        })
    .AddPocketBaseRepositories()
    .BuildServiceProvider();

var articleRepository = services.GetRequiredService<IRepository<ArticleRecord>>();

// 2️⃣ Creating a new record
var article = new
{
    IsPublic = true,
    Name = "Getting Started With PocketBase.Net",
};

var articleRecord = await articleRepository.CreateRecordFrom(article);

// 3️⃣ Updating an existing record
var updatedRecord = await articleRepository.UpdateRecord(
    articleRecord.Id,
    article with { IsPublic = false });

// 4️⃣ Searching for a specific record
var draftsFilter = Filter
    .Field("isDraft")
    .Equal(true)
    .Build();

var draftsArticleRecords = await articleRepository
    .Query()
    .WithFilter(draftsFilter)
    .ExecuteAsync();

// 5️⃣ Deleting a record
foreach (var draft in draftsArticleRecords.Items)
{
    await articleRepository.DeleteRecord(draft.Id);
}

// 6️⃣ Listing all records
var articleRecords = await articleRepository.GetRecords();
```

## Overview

### Registering PocketBase.Net services

PocketBase.Net exposes several classes that can be registered in the dependency injection container with the dedicated `.AddPocketBase` extension method.
This method expects the URL of your PocketBase instance and the credentials of the account the library should use.

```csharp
var administratorCredentials = new PocketBaseClientCredentials
{
    Identity = "...",
    Password = "...",
};

builder.Services.AddPocketBase(
    serverUrl: new Uri(...),
    credentials: administratorCredentials);
```

> [!TIP]
> If the identity you would like to use does not belong to the "users" collection,
> you can tell PocketBase.Net which one to use by specifying `PocketBaseClientCredentials.CollectionName`.

Alternatively, you can customize the configuration from the dedicated lambda:

```csharp
builder.Services.AddPocketBase(
    serverUrl: ...,
    credentials: ...,
    (config) => ... );
```

> [!NOTE]
> By default, PocketBase.NET will attempt to authenticate itself the first time it interacts with your PocketBase instance, if it is not already authenticated.
> If you prefer to manually trigger the authentication, you can disable this behavior by setting `UseSilentAuthentication` to `false`.

### Interacting with PocketBase using Repositories

#### Setup

PocketBase.Net allows you to interact with your entities (or `records`, as they are called in PocketBase) through specialized repositories.

In order to use them, you will have to invoke `AddPocketBaseRepositories` and specify the assembly in which your records are declared if needed.

When added, PocketBase.Net will scan your current assembly (or the one given in parameter) for classes extending `RecordBase`.
If they are not marked with the `[SkipRepositoryCreation]` attribute, a repository will be dynamically registered for them, which you can then use through dependency injection.

```csharp
// 1️⃣ Create your record
public sealed record ArticleRecord : RecordBase;

// 2️⃣ Register the repositories
builder.Services
    .AddPocketBase(...)
    .AddPocketBaseRepositories();

// 3️⃣ Retrieve the article from the dependency injection container
var articleRepository = services.GetRequiredService<IRepository<ArticleRecord>>();
```

<details> <summary>🧪 Excluding a specific class</summary>

If you would like not to create a repository for a specific record, you can opt-out by tagging the class with the `SkipRepositoryCreationAttribute`:

```csharp
[SkipRepositoryCreation]
public sealed record ArticleStatisticRecord : RecordBase;
```
</details>

<details> <summary>🧪 Customizing the repository name</summary>

By default, PocketBase.Net will assume that the collection name of a repository is its camelCased and pluralized name, minus the `-Record` suffix. For instance, the collection name of `ArticleStatisticRecord` will be inferred as `articleStatistics`.

However this behavior can be customized to match your needs using the configuration lambda in the `AddPocketBase` extension method.

This can be especially handy if you would like to target a specific type, or match your own conventions:

```csharp
builder.Services
    .AddPocketBase(
        serverUrl: ...,
        credentials: ...,
        (config) =>
        {
            config.CollectionNamingPipeline.AppendRules([
                new ForRecord<PersonRecord>((_) => "people"),
                new UseTransformation((currentName) => $"collection_{currentName}"),
            ]);
        })
    .AddPocketBaseRepositories();
```

Each rule will be executed in the same order as the one they are registered with.
In the previous example, `ForRecord<>` will be executed before `UseTransformation`.

A few transformations are already provided by the library:

| Rule               | Purpose                                                | Usage                                                    |
|:-------------------|:-------------------------------------------------------|:---------------------------------------------------------|
| `ForRecord`        | Apply a transformation on a specific type              | `new ForRecord<PersonRecord>((_) => "people")`           |
| `SeedWithTypeName` | Use the type name as the collection name               | `new SeedWithTypeName()`                                 |
| `TrimSuffix`       | Trim a suffix from the name (by default `Record`)      | `new TrimSuffix()` or `new TrimSuffix("-suffix")`        |
| `UseCamelCase`     | Change the first letter to lower case                  | `new UseCamelCase()`                                     |
| `UsePlural`        | Add a trailing `s` to the name if there is not one yet | `new UsePlural()`                                        |
| `UseTransformation`| Apply a lambda on the current name                     | `new UseTransformation((name) => $"{name}_transformed")` |

</details>

#### Common operations

##### List/Search for Records

> [!WARNING]
> Some features available in the PocketBase web API are not yet implemented.
> For example, relationships, selected fields and other things are not yet supported but should be in a near future.

Using the repository, you can list the records in the collection using `GetRecords`:

```csharp
var articles = await articleRepository.GetRecords();
```

However, chances are that you will have to search for records satisfying certain conditions.
For those use cases, you can create and configure a dedicated query:

```csharp
var publicArticles = await articleRepository
    .Query()
    .WithFilter("isPublic=true")
    .ExecuteAsync();
```

> [!NOTE]
> You can also customize the pagination of your search result using `WithPagination`.

Alternatively, you can create the filter separately.
This is especially useful to manage complexity when combining multiple conditions and/or nested conditions.

```csharp
// 👇 Equivalent to `isPublic=true && (title~"PocketBase" || "title~".NET")`
var relevantPublicArticlesFilter = Filter
    .Field("isPublic").Equal(true)
    .And().Grouped(
        Filter.Field("title").Like("PocketBase")
        .Or().Field("title").Like(".NET"))
    .Build();

var relevantPublicArticles = await articleRepository
    .Query()
    .WithFilter(relevantPublicArticlesFilter)
    .ExecuteAsync();
```

In the same way, you can also specify how you would like your result sorted, either by directly providing the string,
or by using the provided `SortBuilder`:

```csharp
// 👇 Equivalent to `-createdOn,name`
var oldestFirstSort = Sort
    .ByDescending("createdOn")
    .ThenBy("title")
    .Build();

var articlesByCreationDate = await articleRepository
    .Query()
    .WithSorting(oldestFirstSort)
    .ExecuteAsync();
```

##### Create a record

To create a record, pass the payload to the repository's `CreateRecordFrom` method:

```csharp
var draftArticle = new
{
    IsPublic = false,
    Name = "Getting Started With PocketBase.Net",
};

var draftArticleRecord = await articleRepository.CreateRecordFrom(draftArticle);
```

> [!TIP]
> Most repository operations also have an overload accepting a lambda to be executing when an error occurs:
> ```csharp
> await articleRepository.CreateRecordFrom(article, onError: (payload, error) => ...);
>```

##### Update a record

To update a record, provide the target's id along with its updated content:

```csharp
var publishedArticleRecord = await articleRepository.UpdateRecord(
    draftArticleRecord.Id,
    draftArticle with { IsPublic = true });
```

##### Delete a record

To delete a record, only the record's id is required:

```csharp
await articleRepository.DeleteRecord(publishedArticleRecord.Id);
```

## Contributing

PocketBase.Net is an open-source project and welcomes contributions from the community.
There are several ways you can contribute:

1. **Join the Discussion**: Have a feature request or an idea for improvement? Open a new discussion on the [discussions page](https://github.com/pBouillon/PocketBase.Net/discussions).
2. **Explore Issues**: Check out the [opened issues](https://github.com/pBouillon/PocketBase.Net/issues) to see if there's something you can help with. Bug reports, feature requests, and pull requests are all welcome!
3. **Submit a Pull Request**: If you have code changes to contribute, please submit a pull request. Make sure to follow our coding guidelines, include tests and update the documentation where applicable.

Your contributions help make PocketBase.Net better for everyone!
