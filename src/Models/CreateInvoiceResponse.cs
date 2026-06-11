using System.Text.Json.Serialization;

namespace Heleket.Models
{
    /// <summary>
    /// Legacy invoice result model retained for compatibility.
    /// </summary>
    public class CreateInvoiceInnerResult
    {
        /// <summary>Gets or sets the Heleket payment UUID.</summary>
        [JsonPropertyName("uuid")]
        public string? Uuid { get; set; }

        /// <summary>Gets or sets the merchant order identifier.</summary>
        [JsonPropertyName("order_id")]
        public string? OrderId { get; set; }

        /// <summary>Gets or sets the invoice amount as a string wire value.</summary>
        [JsonPropertyName("amount")]
        public string? Amount { get; set; }

        /// <summary>Gets or sets the payment amount as a string wire value.</summary>
        [JsonPropertyName("payment_amount")]
        public string? PaymentAmount { get; set; }

        /// <summary>Gets or sets the payer amount as a string wire value.</summary>
        [JsonPropertyName("payer_amount")]
        public string? PayerAmount { get; set; }

        /// <summary>Gets or sets the payer currency code.</summary>
        [JsonPropertyName("payer_currency")]
        public string? PayerCurrency { get; set; }

        /// <summary>Gets or sets the invoice currency code.</summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>Gets or sets the sender address.</summary>
        [JsonPropertyName("from")]
        public string? From { get; set; }

        /// <summary>Gets or sets the transaction identifier.</summary>
        [JsonPropertyName("txid")]
        public string? Txid { get; set; }

        /// <summary>Gets or sets the payment status.</summary>
        [JsonPropertyName("payment_status")]
        public string? PaymentStatus { get; set; }

        /// <summary>Gets or sets the hosted invoice URL.</summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>Gets or sets the invoice expiration timestamp.</summary>
        [JsonPropertyName("expired_at")]
        public int? ExpiredAt { get; set; }

        /// <summary>Gets or sets whether the payment is final.</summary>
        [JsonPropertyName("is_final")]
        public bool? IsFinal { get; set; }
    }

    /// <summary>
    /// Legacy invoice response model retained for compatibility.
    /// </summary>
    public class CreateInvoiceResponse
    {
        /// <summary>Gets or sets the Heleket response state.</summary>
        [JsonPropertyName("state")]
        public int? State { get; set; }

        /// <summary>Gets or sets the typed invoice result.</summary>
        [JsonPropertyName("result")]
        public CreateInvoiceInnerResult? Result { get; set; }

        /// <summary>Gets or sets an error message when available.</summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
