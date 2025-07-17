namespace PocketBase.Net.Client.Entities.Filter;

/// <summary>
/// Represents a nested condition in the filter expression.
/// </summary>
public sealed class GroupNode(ITerminalNode nested) : BaseNode, ITerminalNode
{
    /// <summary>
    /// Adds an AND condition to the nested filter expression.
    /// </summary>
    /// <returns>A ConditionNode representing the AND condition.</returns>
    public ConditionNode And()
        => LinkAndReturn(new ConditionNode("&&"));

    /// <summary>
    /// Adds an OR condition to the nested filter expression.
    /// </summary>
    /// <returns>A ConditionNode representing the OR condition.</returns>
    public ConditionNode Or()
        => LinkAndReturn(new ConditionNode("||"));

    /// <summary>
    /// Builds the filter expression string for this node and its subsequent nodes.
    /// </summary>
    /// <returns>The filter expression string.</returns>
    public override string Build()
        => $"{PreviousNode?.Build()}({nested.Build()})";
}
