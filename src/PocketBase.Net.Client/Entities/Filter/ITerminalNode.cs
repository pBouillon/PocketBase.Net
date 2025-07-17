namespace PocketBase.Net.Client.Entities.Filter;

/// <summary>
/// Interface for nodes that can terminate a filter expression.
/// </summary>
public interface ITerminalNode
{
    /// <summary>
    /// Builds the filter expression string.
    /// </summary>
    /// <returns>The filter expression string.</returns>
    string Build();
}
