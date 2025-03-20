using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Shared.Converters;

public class DateOnlyJsonConverter : JsonConverter<DateTime>
{
    private const string DateFormat = "yyyy-MM-dd";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        var date = DateTime.ParseExact(dateString!, DateFormat, null);
        return DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var utcDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        writer.WriteStringValue(utcDate.ToString(DateFormat));
    }
} 