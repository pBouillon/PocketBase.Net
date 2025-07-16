using System.Numerics;

namespace PocketBase.Net.Client;

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

/// <summary>
/// Represents a field in the filter expression.
/// </summary>
public sealed class FieldNode(string fieldName) : BaseNode
{
    /// <summary>
    /// Adds an equality condition to the filter expression.
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the equality condition.</returns>
    public OperatorNode Equal(object value)
        => LinkAndReturn(new OperatorNode("=", value));

    /// <summary>
    /// Adds a non-equality condition to the filter expression.
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the non-equality condition.</returns>
    public OperatorNode NotEqual(object value)
        => LinkAndReturn(new OperatorNode("!=", value));

    /// <summary>
    /// Adds a greater-than condition to the filter expression.
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the greater-than condition.</returns>
    public OperatorNode GreaterThan<TValue>(TValue value)
        where TValue : INumber<TValue>
        => LinkAndReturn(new OperatorNode(">", value));

    /// <summary>
    /// Adds a greater-than-or-equal condition to the filter expression.
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the greater-than-or-equal condition.</returns>
    public OperatorNode GreaterThanOrEqual<TValue>(TValue value)
        where TValue : INumber<TValue>
        => LinkAndReturn(new OperatorNode(">=", value));

    /// <summary>
    /// Adds a less-than condition to the filter expression.
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the less-than condition.</returns>
    public OperatorNode LessThan<TValue>(TValue value)
        where TValue : INumber<TValue>
        => LinkAndReturn(new OperatorNode("<", value));

    /// <summary>
    /// Adds a less-than-or-equal condition to the filter expression.
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the less-than-or-equal condition.</returns>
    public OperatorNode LessThanOrEqual<TValue>(TValue value)
        where TValue : INumber<TValue>
        => LinkAndReturn(new OperatorNode("<=", value));

    /// <summary>
    /// Adds a like condition to the filter expression.
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the like condition.</returns>
    public OperatorNode Like(string value)
        => LinkAndReturn(new OperatorNode("~", value));

    /// <summary>
    /// Adds a not-like condition to the filter expression.
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the not-like condition.</returns>
    public OperatorNode NotLike(string value)
        => LinkAndReturn(new OperatorNode("!~", value));

    /// <summary>
    /// Adds an any-equal condition to the filter expression (for array fields).
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the any-equal condition.</returns>
    public OperatorNode AnyEqual(object value)
        => LinkAndReturn(new OperatorNode("?=", value));

    /// <summary>
    /// Adds an any-not-equal condition to the filter expression (for array fields).
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the any-not-equal condition.</returns>
    public OperatorNode AnyNotEqual(object value)
        => LinkAndReturn(new OperatorNode("?!=", value));

    /// <summary>
    /// Adds an any-greater-than condition to the filter expression (for array fields).
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the any-greater-than condition.</returns>
    public OperatorNode AnyGreaterThan<TValue>(TValue value) where TValue : INumber<TValue>
        => LinkAndReturn(new OperatorNode("?>", value));

    /// <summary>
    /// Adds an any-greater-than-or-equal condition to the filter expression (for array fields).
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the any-greater-than-or-equal condition.</returns>
    public OperatorNode AnyGreaterThanOrEqual<TValue>(TValue value) where TValue : INumber<TValue>
        => LinkAndReturn(new OperatorNode("?>=", value));

    /// <summary>
    /// Adds an any-less-than condition to the filter expression (for array fields).
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the any-less-than condition.</returns>
    public OperatorNode AnyLessThan<TValue>(TValue value) where TValue : INumber<TValue>
        => LinkAndReturn(new OperatorNode("?<", value));

    /// <summary>
    /// Adds an any-less-than-or-equal condition to the filter expression (for array fields).
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the any-less-than-or-equal condition.</returns>
    public OperatorNode AnyLessThanOrEqual<TValue>(TValue value) where TValue : INumber<TValue>
        => LinkAndReturn(new OperatorNode("?<=", value));

    /// <summary>
    /// Adds an any-like condition to the filter expression (for array fields).
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the any-like condition.</returns>
    public OperatorNode AnyLike(string value)
        => LinkAndReturn(new OperatorNode("?~", value));

    /// <summary>
    /// Adds an any-not-like condition to the filter expression (for array fields).
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    /// <returns>An OperatorNode representing the any-not-like condition.</returns>
    public OperatorNode AnyNotLike(string value)
        => LinkAndReturn(new OperatorNode("?!~", value));

    /// <summary>
    /// Builds the filter expression string for this node and its subsequent nodes.
    /// </summary>
    /// <returns>The filter expression string.</returns>
    public override string Build()
        => $"{PreviousNode?.Build()}{fieldName}";
}

/// <summary>
/// Represents an operator in the filter expression.
/// </summary>
public sealed class OperatorNode(string @operator, object value) : BaseNode, ITerminalNode
{
    /// <summary>
    /// Adds an AND condition to the filter expression.
    /// </summary>
    /// <returns>A ConditionNode representing the AND condition.</returns>
    public ConditionNode And()
        => LinkAndReturn(new ConditionNode("&&"));

    /// <summary>
    /// Adds an OR condition to the filter expression.
    /// </summary>
    /// <returns>A ConditionNode representing the OR condition.</returns>
    public ConditionNode Or()
        => LinkAndReturn(new ConditionNode("||"));

    /// <summary>
    /// Builds the filter expression string for this node and its subsequent nodes.
    /// </summary>
    /// <returns>The filter expression string.</returns>
    public override string Build()
        => $"{PreviousNode?.Build()}{@operator}" + value switch
        {
            string or char => $"\"{value}\"",
            bool booleanValue => booleanValue ? "true" : "false",
            _ => $"{value}",
        };
}

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
