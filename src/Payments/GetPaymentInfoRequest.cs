using System.Text.Json.Serialization;

namespace Heleket.Payments;

/// <summary>
/// Request body for retrieving Heleket payment information.
/// </summary>
public sealed class GetPaymentInfoRequest
{
    /// <summary>Gets the Heleket payment UUID.</summary>
    [JsonPropertyName("uuid")]
    public string? Uuid { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonPropertyName("order_id")]
    public string? OrderId { get; init; }
}
