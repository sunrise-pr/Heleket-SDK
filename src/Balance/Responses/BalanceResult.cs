using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heleket.Balance.Responses;

/// <summary>
/// Represents Heleket balance information.
/// </summary>
public sealed class BalanceResult
{
    /// <summary>Gets balances when returned as a list.</summary>
    [JsonProperty("balances")]
    public IReadOnlyList<BalanceAccount>? Balances { get; init; }

    /// <summary>Gets merchant or business balances when returned separately.</summary>
    [JsonProperty("merchant")]
    public IReadOnlyList<BalanceAccount>? Merchant { get; init; }

    /// <summary>Gets business balances when returned separately.</summary>
    [JsonProperty("business")]
    public IReadOnlyList<BalanceAccount>? Business { get; init; }

    /// <summary>Gets personal balances when returned separately.</summary>
    [JsonProperty("personal")]
    public IReadOnlyList<BalanceAccount>? Personal { get; init; }

    /// <summary>Gets additional JSON fields returned by Heleket.</summary>
    [JsonExtensionData]
    public IDictionary<string, JToken>? ExtensionData { get; init; }
}

/// <summary>
/// Represents a currency balance row.
/// </summary>
public sealed class BalanceAccount
{
    /// <summary>Gets the currency code.</summary>
    [JsonProperty("currency")]
    public string? Currency { get; init; }

    /// <summary>Gets the blockchain network code when present.</summary>
    [JsonProperty("network")]
    public string? Network { get; init; }

    /// <summary>Gets the balance amount as a string wire value.</summary>
    [JsonProperty("balance")]
    public string? Balance { get; init; }

    /// <summary>Gets the available amount as a string wire value.</summary>
    [JsonProperty("available")]
    public string? Available { get; init; }

    /// <summary>Gets the hold amount as a string wire value.</summary>
    [JsonProperty("hold")]
    public string? Hold { get; init; }

    /// <summary>Gets additional JSON fields returned by Heleket.</summary>
    [JsonExtensionData]
    public IDictionary<string, JToken>? ExtensionData { get; init; }
}
