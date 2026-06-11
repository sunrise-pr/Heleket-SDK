using Newtonsoft.Json;

namespace Heleket.Payments.Requests;

/// <summary>
/// Request body for resending a Heleket payment webhook.
/// </summary>
public sealed class ResendPaymentWebhookRequest
{
    /// <summary>Gets the Heleket payment UUID.</summary>
    [JsonProperty("uuid")]
    public string? Uuid { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonProperty("order_id")]
    public string? OrderId { get; init; }
}
