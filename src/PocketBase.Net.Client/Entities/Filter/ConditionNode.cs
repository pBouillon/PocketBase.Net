namespace PocketBase.Net.Client.Entities.Filter;

/// <summary>
/// Represents a logical condition in the filter expression.
/// </summary>
public sealed class ConditionNode(string symbol) : BaseNode
{
    /// <summary>
    /// Starts a new field condition in the filter expression.
    /// </summary>
    /// <param name="fieldName">The name of the field to filter.</param>
    /// <returns>A FieldNode representing the field in the filter expression.</returns>
    public FieldNode Field(string fieldName)
        => LinkAndReturn(new FieldNode(fieldName));

    /// <summary>
    /// Starts a new nested condition in the filter expression.
    /// </summary>
    /// <param name="nested">The nested filter expression.</param>
    /// <returns>A NestingNode representing the nested condition.</returns>
    public GroupNode Grouped(ITerminalNode nested)
        => LinkAndReturn(new GroupNode(nested));

    /// <summary>
    /// Builds the filter expression string for this node and its subsequent nodes.
    /// </summary>
    /// <returns>The filter expression string.</returns>
    public override string Build()
        => $"{PreviousNode?.Build()} {symbol} ";
}
