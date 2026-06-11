using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heleket.Payments.Responses;

/// <summary>
/// Represents a Heleket payment service entry.
/// </summary>
public sealed class PaymentService
{
    /// <summary>Gets the payment currency code.</summary>
    [JsonProperty("currency")]
    public string? Currency { get; init; }

    /// <summary>Gets the blockchain network code.</summary>
    [JsonProperty("network")]
    public string? Network { get; init; }

    /// <summary>Gets the payment method code when returned by Heleket.</summary>
    [JsonProperty("method")]
    public string? Method { get; init; }

    /// <summary>Gets whether the service is available.</summary>
    [JsonProperty("is_available")]
    public bool? IsAvailable { get; init; }

    /// <summary>Gets the minimum amount as a string wire value.</summary>
    [JsonProperty("min_amount")]
    public string? MinAmount { get; init; }

    /// <summary>Gets the maximum amount as a string wire value.</summary>
    [JsonProperty("max_amount")]
    public string? MaxAmount { get; init; }

    /// <summary>Gets service limits when returned as a nested object.</summary>
    [JsonProperty("limit")]
    public PaymentServiceLimit? Limit { get; init; }

    /// <summary>Gets additional JSON fields returned by Heleket.</summary>
    [JsonExtensionData]
    public IDictionary<string, JToken>? ExtensionData { get; init; }
}

/// <summary>
/// Represents payment service limits.
/// </summary>
public sealed class PaymentServiceLimit
{
    /// <summary>Gets the minimum amount as a string wire value.</summary>
    [JsonProperty("min_amount")]
    public string? MinAmount { get; init; }

    /// <summary>Gets the maximum amount as a string wire value.</summary>
    [JsonProperty("max_amount")]
    public string? MaxAmount { get; init; }

    /// <summary>Gets additional JSON fields returned by Heleket.</summary>
    [JsonExtensionData]
    public IDictionary<string, JToken>? ExtensionData { get; init; }
}
