using Newtonsoft.Json;

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
        [JsonProperty("currency")]
        public string Currency { get; set; } = currency;

        /// <summary>Gets or sets the optional network code.</summary>
        [JsonProperty("network")]
        public string? Network { get; set; } = network;
    }
}
