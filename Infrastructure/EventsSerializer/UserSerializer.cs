using System.Text.Json;
using System.Text.Json.Serialization;
using Common.ValueObject;

namespace Infrastructure.EventsSerializer;
public class UserJsonConverter : JsonConverter<User>
{
    public override User Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;
        var name = jsonObject.GetProperty("Name").GetString();
        var email =  Email.CreateInstance(jsonObject.GetProperty("Email").GetProperty("EmailValue").GetString());
        var address =  Address.CreateInstance(jsonObject.GetProperty("Address").GetProperty("AddressValue").GetString());
        if (email.IsFailure)
            throw new JsonException(email.Message);
        // Assuming User has a static factory method `CreateInstance`
        return User.CreateInstance(email.Value,name,jsonObject.GetProperty("PhoneNumber").GetString()!,address.Value).Value;
    }

    public override void Write(Utf8JsonWriter writer, User value, JsonSerializerOptions options)
    {
        /*
        writer.WriteStartObject();
        writer.WriteString("Name", value.Name);
        writer.WriteString("Email", value.Email);
        writer.WriteEndObject();
        */
    }
}