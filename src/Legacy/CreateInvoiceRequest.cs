using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Heleket.Legacy
{
    /// <summary>
    /// Legacy request model for creating a Heleket invoice.
    /// </summary>
    public class CreateInvoiceRequest
    {
        /// <summary>
        /// Creates a legacy invoice request.
        /// </summary>
        /// <param name="amount">The invoice amount. Prefer the new string-based payment DTOs for public SDK usage.</param>
        /// <param name="orderId">The merchant order identifier.</param>
        /// <param name="urlCallback">The optional callback URL.</param>
        public CreateInvoiceRequest(double amount, string orderId, string? urlCallback)
        {
            Amount = amount.ToString(System.Globalization.CultureInfo.InvariantCulture);
            OrderId = orderId;
            Currency = "USD";
            ToCurrency = null;
            UrlCallback = urlCallback;
        }

        /// <summary>
        /// Gets or sets the invoice amount as a string wire value.
        /// </summary>
        [JsonProperty("amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Gets or sets the invoice currency code.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the merchant order identifier.
        /// </summary>
        [JsonProperty("order_id")]
        [StringLength(128, MinimumLength = 1)]
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the optional webhook callback URL.
        /// </summary>
        [JsonProperty("url_callback")]
        [StringLength(255, MinimumLength = 6)]
        public string? UrlCallback { get; set; }

        /// <summary>
        /// Gets or sets the optional target cryptocurrency for conversion.
        /// </summary>
        [JsonProperty("to_currency")]
        public string? ToCurrency { get; set; }
    }
}
