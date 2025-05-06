using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Events;
using Common.Serializer;

namespace Infrastructure.EventsSerializer;

public class EventJsonConverter: JsonConverter<DomainEventBase>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableFrom(typeof(DomainEventBase));
    }
    public override DomainEventBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out var doc))
            throw new JsonException($"Can not Convert Type {typeof(JsonDocument)}");

        if (!doc.RootElement.TryGetProperty("Type", out var type))
            throw new JsonException($"Type not Complete {typeof(JsonDocument)}");
        var disc = type.GetString();
        var json = doc.RootElement.GetRawText();
        return disc switch
        {
            //nameof(PoCreatedEventBase) => JsonSerializer.Deserialize<PoCreatedEventBase>(json, GetCustomerOptions(options)),
            nameof(OrderBeingShipped) => JsonSerializer.Deserialize<OrderBeingShipped>(json, options),
            nameof(OrderShipped) => JsonSerializer.Deserialize<OrderShipped>(json, options),
            nameof(OrderClosed) => JsonSerializer.Deserialize<OrderClosed>(json, options),
            _ => throw new JsonException($"Event Type {disc} not supported yet!")
        };

    }

    private JsonSerializerOptions GetCustomerOptions(JsonSerializerOptions defaultOption)
    {
        JsonSerializerOptions @new = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        foreach (var converter in defaultOption.Converters)
        {
            @new.Converters.Add(converter);
        }
        @new.Converters
            .Add(new MoneyJsonConverter());
        @new.Converters
            .Add(new UserJsonConverter());
        @new.PropertyNameCaseInsensitive = true;
        return @new;
    }

    public override void Write(Utf8JsonWriter writer, DomainEventBase value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
