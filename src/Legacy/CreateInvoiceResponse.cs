using Newtonsoft.Json;

namespace Heleket.Legacy
{
    /// <summary>
    /// Legacy invoice result model retained for compatibility.
    /// </summary>
    public class CreateInvoiceInnerResult
    {
        /// <summary>Gets or sets the Heleket payment UUID.</summary>
        [JsonProperty("uuid")]
        public string? Uuid { get; set; }

        /// <summary>Gets or sets the merchant order identifier.</summary>
        [JsonProperty("order_id")]
        public string? OrderId { get; set; }

        /// <summary>Gets or sets the invoice amount as a string wire value.</summary>
        [JsonProperty("amount")]
        public string? Amount { get; set; }

        /// <summary>Gets or sets the payment amount as a string wire value.</summary>
        [JsonProperty("payment_amount")]
        public string? PaymentAmount { get; set; }

        /// <summary>Gets or sets the payer amount as a string wire value.</summary>
        [JsonProperty("payer_amount")]
        public string? PayerAmount { get; set; }

        /// <summary>Gets or sets the payer currency code.</summary>
        [JsonProperty("payer_currency")]
        public string? PayerCurrency { get; set; }

        /// <summary>Gets or sets the invoice currency code.</summary>
        [JsonProperty("currency")]
        public string? Currency { get; set; }

        /// <summary>Gets or sets the sender address.</summary>
        [JsonProperty("from")]
        public string? From { get; set; }

        /// <summary>Gets or sets the transaction identifier.</summary>
        [JsonProperty("txid")]
        public string? Txid { get; set; }

        /// <summary>Gets or sets the payment status.</summary>
        [JsonProperty("payment_status")]
        public string? PaymentStatus { get; set; }

        /// <summary>Gets or sets the hosted invoice URL.</summary>
        [JsonProperty("url")]
        public string? Url { get; set; }

        /// <summary>Gets or sets the invoice expiration timestamp.</summary>
        [JsonProperty("expired_at")]
        public int? ExpiredAt { get; set; }

        /// <summary>Gets or sets whether the payment is final.</summary>
        [JsonProperty("is_final")]
        public bool? IsFinal { get; set; }
    }

    /// <summary>
    /// Legacy invoice response model retained for compatibility.
    /// </summary>
    public class CreateInvoiceResponse
    {
        /// <summary>Gets or sets the Heleket response state.</summary>
        [JsonProperty("state")]
        public int? State { get; set; }

        /// <summary>Gets or sets the typed invoice result.</summary>
        [JsonProperty("result")]
        public CreateInvoiceInnerResult? Result { get; set; }

        /// <summary>Gets or sets an error message when available.</summary>
        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}
