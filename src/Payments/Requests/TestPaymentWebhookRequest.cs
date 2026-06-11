using Newtonsoft.Json;

namespace Heleket.Payments.Requests;

/// <summary>
/// Request body for sending a Heleket payment test webhook.
/// </summary>
public sealed class TestPaymentWebhookRequest
{
    /// <summary>Gets the Heleket payment UUID.</summary>
    [JsonProperty("uuid")]
    public string? Uuid { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonProperty("order_id")]
    public string? OrderId { get; init; }

    /// <summary>Gets the callback URL for the test webhook when supplied.</summary>
    [JsonProperty("url_callback")]
    public string? UrlCallback { get; init; }
}
