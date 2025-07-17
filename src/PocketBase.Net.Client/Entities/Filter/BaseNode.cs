namespace PocketBase.Net.Client.Entities.Filter;

/// <summary>
/// Base class for all nodes in the filter expression tree.
/// </summary>
public abstract class BaseNode
{
    internal BaseNode? PreviousNode { get; private set; }

    /// <summary>
    /// Links the current node to the next node and returns it.
    /// </summary>
    /// <typeparam name="TNode">The type of the next node.</typeparam>
    /// <param name="node">The next node to link.</param>
    /// <returns>The linked node.</returns>
    protected TNode LinkAndReturn<TNode>(TNode node)
        where TNode : BaseNode
    {
        node.PreviousNode = this;
        return node;
    }

    /// <summary>
    /// Builds the filter expression string for this node and its subsequent nodes.
    /// </summary>
    /// <returns>The filter expression string.</returns>
    public abstract string Build();
}
