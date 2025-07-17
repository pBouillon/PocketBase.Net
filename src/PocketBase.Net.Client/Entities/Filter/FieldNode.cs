using System.Numerics;

namespace PocketBase.Net.Client.Entities.Filter;

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
