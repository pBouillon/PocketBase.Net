namespace PocketBase.Net.Client.Entities;

/// <summary>
/// Represents a sort specification for ordering query results in PocketBase.
/// This class provides a fluent interface for building sort expressions.
/// </summary>
public sealed class Sort
{
    private readonly Queue<string> _sortSections = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Sort"/> class with an initial sort field.
    /// </summary>
    /// <param name="seed">The initial sort field specification.</param>
    private Sort(string seed)
        => _sortSections.Enqueue(seed);

    /// <summary>
    /// Starts a sort specification for ascending order.
    /// </summary>
    /// <param name="fieldName">The name of the field to sort by.</param>
    /// <returns>A new <see cref="Sort"/> instance configured to sort by the specified field in ascending order.</returns>
    public static Sort By(string fieldName)
        => new(fieldName);

    /// <summary>
    /// Starts a sort specification for descending order.
    /// </summary>
    /// <param name="fieldName">The name of the field to sort by.</param>
    /// <returns>A new <see cref="Sort"/> instance configured to sort by the specified field in descending order.</returns>
    public static Sort ByDescending(string fieldName)
        => new(AsDescendingField(fieldName));

    /// <summary>
    /// Adds another field to sort by in ascending order.
    /// </summary>
    /// <param name="fieldName">The name of the additional field to sort by.</param>
    /// <returns>The current <see cref="Sort"/> instance for method chaining.</returns>
    public Sort ThenBy(string fieldName)
    {
        _sortSections.Enqueue(fieldName);
        return this;
    }

    /// <summary>
    /// Adds another field to sort by in descending order.
    /// </summary>
    /// <param name="fieldName">The name of the additional field to sort by.</param>
    /// <returns>The current <see cref="Sort"/> instance for method chaining.</returns>
    public Sort ThenByDescending(string fieldName)
    {
        _sortSections.Enqueue(AsDescendingField(fieldName));
        return this;
    }

    /// <summary>
    /// Converts a field name to a descending sort field specification.
    /// </summary>
    /// <param name="fieldName">The field name to convert.</param>
    /// <returns>A string representing the descending sort for the field.</returns>
    private static string AsDescendingField(string fieldName)
        => '-' + fieldName;

    /// <summary>
    /// Builds the final sort expression string.
    /// </summary>
    /// <returns>A comma-separated string representing the complete sort specification in PocketBase format.</returns>
    public string Build()
        => string.Join(',', _sortSections);
}
