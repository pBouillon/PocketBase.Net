namespace PocketBase.Net.Client.Entities.Filter;

/// <summary>
/// Static class providing entry points for creating filter expressions.
/// </summary>
public static class Filter
{
    /// <summary>
    /// Starts a new filter expression with the specified field name.
    /// </summary>
    /// <param name="fieldName">The name of the field to filter.</param>
    /// <returns>A FieldNode representing the field in the filter expression.</returns>
    public static FieldNode Field(string fieldName)
        => new(fieldName);
}
