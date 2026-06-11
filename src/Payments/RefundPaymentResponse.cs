using System.Text.Json;
using System.Text.Json.Serialization;

namespace Heleket.Payments;

/// <summary>
/// Refund response payload returned by Heleket.
/// </summary>
public sealed class RefundPaymentResponse
{
    /// <summary>Gets the Heleket payment UUID.</summary>
    [JsonPropertyName("uuid")]
    public string? Uuid { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonPropertyName("order_id")]
    public string? OrderId { get; init; }

    /// <summary>Gets the refund or payment status.</summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>Gets the refunded amount as a string wire value.</summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>Gets additional JSON fields returned by Heleket.</summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; init; }
}
