using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common.ValueObject;

namespace Infrastructure.Serializer
{
    public class AddressJsonConverter : JsonConverter<Address>
    {
        public override Address Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var root = jsonDoc.RootElement;

            if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty("AddressValue", out var valueElement))
            {
                throw new JsonException("Invalid JSON format for Address.");
            }

            var addressValue = valueElement.GetString();

            var result = Address.CreateInstance(addressValue);
            if (!result.IsSuccess)
            {
                throw new JsonException($"Invalid Address: {result.Message}");
            }

            return result.Value;
        }

        public override void Write(Utf8JsonWriter writer, Address value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("AddressValue", value.AddressValue);
            writer.WriteEndObject();
        }
    }
}
