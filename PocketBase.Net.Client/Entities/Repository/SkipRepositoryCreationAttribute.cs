namespace PocketBase.Net.Client.Entities.Repository;

/// <summary>
/// Attribute that can be applied to record types to skip the automatic <see cref="IRepository{TRecord}"/> creation
/// for a given <see cref="Records.RecordBase"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class SkipRepositoryCreationAttribute : Attribute
{ }
