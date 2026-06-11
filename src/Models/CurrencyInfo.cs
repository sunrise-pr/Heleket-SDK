using System.Text.Json;
using System.Text.Json.Serialization;

namespace Heleket.Models
{
    /// <summary>
    /// Legacy currency/network pair retained for compatibility.
    /// </summary>
    /// <param name="currency">The currency code.</param>
    /// <param name="network">The optional network code.</param>
    public class CurrencyInfo(string currency, string? network)
    {
        /// <summary>Gets or sets the currency code.</summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = currency;

        /// <summary>Gets or sets the optional network code.</summary>
        [JsonPropertyName("network")]
        public string? Network { get; set; } = network;
    }
}
