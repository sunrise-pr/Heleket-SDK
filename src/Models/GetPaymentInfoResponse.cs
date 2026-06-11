using Newtonsoft.Json;

namespace Heleket.Models
{
    /// <summary>
    /// Legacy payment info result model retained for compatibility.
    /// </summary>
    public class GetPaymentInfoInnerResponse
    {
        /// <summary>Gets or sets the Heleket payment UUID.</summary>
        [JsonProperty("uuid")]
        public string? Uuid { get; set; }

        /// <summary>Gets or sets the merchant order identifier.</summary>
        [JsonProperty("order_id")]
        public string? OrderId { get; set; }

        /// <summary>Gets or sets the invoice amount.</summary>
        [JsonProperty("amount")]
        public string? Amount { get; set; }

        /// <summary>Gets or sets the paid amount.</summary>
        [JsonProperty("payment_amount")]
        public string? PaymentAmount { get; set; }

        /// <summary>Gets or sets the payer amount.</summary>
        [JsonProperty("payer_amount")]
        public string? PayerAmount { get; set; }

        /// <summary>Gets or sets the applied discount percentage.</summary>
        [JsonProperty("discount_percent")]
        public decimal? DiscountPercent { get; set; }

        /// <summary>Gets or sets the applied discount amount.</summary>
        [JsonProperty("discount")]
        public string? Discount { get; set; }

        /// <summary>Gets or sets the payer currency code.</summary>
        [JsonProperty("payer_currency")]
        public string? PayerCurrency { get; set; }

        /// <summary>Gets or sets the invoice currency code.</summary>
        [JsonProperty("currency")]
        public string? Currency { get; set; }

        /// <summary>Gets or sets payment comments.</summary>
        [JsonProperty("comments")]
        public string? Comments { get; set; }

        /// <summary>Gets or sets the merchant amount.</summary>
        [JsonProperty("merchant_amount")]
        public string? MerchantAmount { get; set; }

        /// <summary>Gets or sets the blockchain network code.</summary>
        [JsonProperty("network")]
        public string? Network { get; set; }

        /// <summary>Gets or sets the payment address.</summary>
        [JsonProperty("address")]
        public string? Address { get; set; }

        /// <summary>Gets or sets the sender address.</summary>
        [JsonProperty("from")]
        public string? From { get; set; }

        /// <summary>Gets or sets the blockchain transaction identifier.</summary>
        [JsonProperty("txid")]
        public string? Txid { get; set; }

        /// <summary>Gets or sets the payment status field.</summary>
        [JsonProperty("payment_status")]
        public string? PaymentStatus { get; set; }

        /// <summary>Gets or sets the hosted invoice URL.</summary>
        [JsonProperty("url")]
        public string? Url { get; set; }

        /// <summary>Gets or sets the invoice expiration timestamp.</summary>
        [JsonProperty("expired_at")]
        public long? ExpiredAt { get; set; }

        /// <summary>Gets or sets the status field.</summary>
        [JsonProperty("status")]
        public string? Status { get; set; }

        /// <summary>Gets or sets whether the payment is final.</summary>
        [JsonProperty("is_final")]
        public bool? IsFinal { get; set; }

        /// <summary>Gets or sets merchant-defined additional data.</summary>
        [JsonProperty("additional_data")]
        public string? AdditionalData { get; set; }

        /// <summary>Gets or sets the creation timestamp.</summary>
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>Gets or sets the update timestamp.</summary>
        [JsonProperty("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Legacy payment info response model retained for compatibility.
    /// </summary>
    public class GetPaymentInfoResponse
    {
        /// <summary>Gets or sets the Heleket response state.</summary>
        [JsonProperty("state")]
        public int State { get; set; }

        /// <summary>Gets or sets the payment info result.</summary>
        [JsonProperty("result")]
        public GetPaymentInfoInnerResponse? Result { get; set; }
    }
}
