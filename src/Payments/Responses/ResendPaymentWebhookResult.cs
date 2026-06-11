using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heleket.Payments.Responses;

/// <summary>
/// Represents the result returned after requesting payment webhook resend.
/// </summary>
public sealed class ResendPaymentWebhookResult
{
    /// <summary>Gets whether the resend request was accepted when returned by Heleket.</summary>
    [JsonProperty("is_sent")]
    public bool? IsSent { get; init; }

    /// <summary>Gets the message returned by Heleket when present.</summary>
    [JsonProperty("message")]
    public string? Message { get; init; }

    /// <summary>Gets additional JSON fields returned by Heleket.</summary>
    [JsonExtensionData]
    public IDictionary<string, JToken>? ExtensionData { get; init; }
}
