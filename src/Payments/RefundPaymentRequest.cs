using System.Text.Json.Serialization;

namespace Heleket.Payments;

/// <summary>
/// Request body for refunding a Heleket payment.
/// </summary>
public sealed class RefundPaymentRequest
{
    /// <summary>Gets the Heleket payment UUID.</summary>
    [JsonPropertyName("uuid")]
    public string? Uuid { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonPropertyName("order_id")]
    public string? OrderId { get; init; }

    /// <summary>Gets the destination refund address.</summary>
    [JsonPropertyName("address")]
    public required string Address { get; init; }

    /// <summary>Gets whether refund fees should be subtracted from the refund amount.</summary>
    [JsonPropertyName("is_subtract")]
    public required bool IsSubtract { get; init; }

    /// <summary>Gets the optional refund amount as a string wire value.</summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }
}
