using System.Text.Json;
using System.Text.Json.Serialization;

namespace PocketBase.Net.Client;

/// <summary>
/// Implement the conversion of PocketBase's DateTime format (<c>yyyy-MM-dd HH:mm:ss.sssZ</c>).
/// </summary>
/// <remarks>
/// This converter is needed since PocketBase is using a variant of the ISO 8601 (<c>yyyy-MM-ddTHH:mm:ss.sssZ</c>)
/// that does not use <c>T</c> as a separator but a space.
/// </remarks>
public sealed class PocketBaseDateTimeConverter : JsonConverter<DateTime>
{
    /// <summary>
    /// PocketBase's custom DateTime format
    /// </summary>
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fffZ";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.ParseExact(reader.GetString() ?? string.Empty, DateTimeFormat, provider: null);

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(DateTimeFormat));
}