using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heleket.Payments.Responses;

/// <summary>
/// Refund response payload returned by Heleket.
/// </summary>
[JsonConverter(typeof(RefundPaymentResponseConverter))]
public sealed class RefundPaymentResponse
{
    /// <summary>Gets the raw array items returned by the documented refund success response.</summary>
    [JsonIgnore]
    public IReadOnlyList<JToken> Items { get; init; } = Array.Empty<JToken>();

    /// <summary>Gets the Heleket payment UUID.</summary>
    [JsonProperty("uuid")]
    public string? Uuid { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonProperty("order_id")]
    public string? OrderId { get; init; }

    /// <summary>Gets the refund or payment status.</summary>
    [JsonProperty("status")]
    public string? Status { get; init; }

    /// <summary>Gets the refunded amount as a string wire value.</summary>
    [JsonProperty("amount")]
    public string? Amount { get; init; }

    /// <summary>Gets additional JSON fields returned by Heleket.</summary>
    [JsonExtensionData]
    public IDictionary<string, JToken>? ExtensionData { get; init; }
}

internal sealed class RefundPaymentResponseConverter : JsonConverter<RefundPaymentResponse>
{
    public override RefundPaymentResponse? ReadJson(
        JsonReader reader,
        Type objectType,
        RefundPaymentResponse? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        if (token is JArray array)
        {
            return new RefundPaymentResponse
            {
                Items = array.ToArray()
            };
        }

        if (token is JObject obj)
        {
            using var objectReader = obj.CreateReader();
            var response = new RefundPaymentResponse();
            serializer.Populate(objectReader, response);
            return response;
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, RefundPaymentResponse? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value?.Items ?? Array.Empty<JToken>());
    }
}
