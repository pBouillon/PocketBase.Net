namespace PocketBase.Net.Client.Configuration;

/// <summary>
/// Specifies validation behavior for record operations.
/// </summary>
[Flags]
public enum RecordOperationBehavior
{
    /// <summary>
    /// Default behavior: Strict validation that doesn't ignore any issues.
    /// </summary>
    Strict = 0,

    /// <summary>
    /// Ignores unknown properties in the input.
    /// </summary>
    IgnoreUnknownProperties = 1,

    /// <summary>
    /// Ignores property type mismatches in the input.
    /// </summary>
    IgnorePropertyTypeMismatches = 2,

    /// <summary>
    /// Combination that ignores both unknown properties and type mismatches.
    /// </summary>
    IgnoreAll = IgnoreUnknownProperties | IgnorePropertyTypeMismatches
}