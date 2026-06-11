using Newtonsoft.Json;

namespace Heleket.Payments.Requests;

/// <summary>
/// Request body for refunding a Heleket payment.
/// </summary>
public sealed class RefundPaymentRequest
{
    /// <summary>Gets the Heleket payment UUID.</summary>
    [JsonProperty("uuid")]
    public string? Uuid { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonProperty("order_id")]
    public string? OrderId { get; init; }

    /// <summary>Gets the destination refund address.</summary>
    [JsonProperty("address")]
    public required string Address { get; init; }

    /// <summary>Gets whether refund fees should be subtracted from the refund amount.</summary>
    [JsonProperty("is_subtract")]
    public required bool IsSubtract { get; init; }

    /// <summary>Gets the optional refund amount as a string wire value.</summary>
    [JsonProperty("amount")]
    public string? Amount { get; init; }
}
