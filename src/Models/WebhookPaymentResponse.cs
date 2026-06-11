using Newtonsoft.Json;

namespace Heleket.Models
{
    /// <summary>
    /// Legacy conversion information model retained for compatibility.
    /// </summary>
    public class ConversionInfo
    {
        /// <summary>Gets or sets the target conversion currency.</summary>
        [JsonProperty("to_currency")]
        public string? ToCurrency { get; set; }

        /// <summary>Gets or sets the conversion commission.</summary>
        [JsonProperty("commission")]
        public string? Commission { get; set; }

        /// <summary>Gets or sets the conversion rate.</summary>
        [JsonProperty("rate")]
        public string? Rate { get; set; }

        /// <summary>Gets or sets the converted amount.</summary>
        [JsonProperty("amount")]
        public string? Amount { get; set; }
    }

    /// <summary>
    /// Legacy payment webhook payload retained for compatibility.
    /// </summary>
    public class WebhookPaymentResponse
    {
        /// <summary>Gets or sets the webhook type.</summary>
        [JsonProperty("type")]
        public string? Type { get; set; }

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

        /// <summary>Gets or sets the paid amount in USD.</summary>
        [JsonProperty("payment_amount_usd")]
        public string? PaymentAmountUsd { get; set; }

        /// <summary>Gets or sets the merchant amount.</summary>
        [JsonProperty("merchant_amount")]
        public string? MerchantAmount { get; set; }

        /// <summary>Gets or sets the commission.</summary>
        [JsonProperty("commission")]
        public string? Commission { get; set; }

        /// <summary>Gets or sets whether the webhook is final.</summary>
        [JsonProperty("is_final")]
        public bool? IsFinal { get; set; }

        /// <summary>Gets or sets the payment status.</summary>
        [JsonProperty("status")]
        public string? Status { get; set; }

        /// <summary>Gets or sets the sender address.</summary>
        [JsonProperty("from")]
        public string? From { get; set; }

        /// <summary>Gets or sets the wallet address UUID.</summary>
        [JsonProperty("wallet_address_uuid")]
        public string? WalletAddressUuid { get; set; }

        /// <summary>Gets or sets the network code.</summary>
        [JsonProperty("network")]
        public string? Network { get; set; }

        /// <summary>Gets or sets the invoice currency code.</summary>
        [JsonProperty("currency")]
        public string? Currency { get; set; }

        /// <summary>Gets or sets the payer currency code.</summary>
        [JsonProperty("payer_currency")]
        public string? PayerCurrency { get; set; }

        /// <summary>Gets or sets merchant-defined additional data.</summary>
        [JsonProperty("additional_data")]
        public string? AdditionalData { get; set; }

        /// <summary>Gets or sets conversion information.</summary>
        [JsonProperty("convert")]
        public ConversionInfo? Convert { get; set; }

        /// <summary>Gets or sets the transaction identifier.</summary>
        [JsonProperty("txid")]
        public string? Txid { get; set; }

        /// <summary>Gets or sets the received signature.</summary>
        [JsonProperty("sign")]
        public string? Sign { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// Legacy payment webhook payload without the signature field.
    /// </summary>
    public class WebhookPaymentResponseNoSign(WebhookPaymentResponse webhook)
    {
        /// <summary>Gets or sets the webhook type.</summary>
        [JsonProperty("type")]
        public string? Type { get; set; } = webhook.Type;

        /// <summary>Gets or sets the Heleket payment UUID.</summary>
        [JsonProperty("uuid")]
        public string? Uuid { get; set; } = webhook.Uuid;

        /// <summary>Gets or sets the merchant order identifier.</summary>
        [JsonProperty("order_id")]
        public string? OrderId { get; set; } = webhook.OrderId;

        /// <summary>Gets or sets the invoice amount.</summary>
        [JsonProperty("amount")]
        public string? Amount { get; set; } = webhook.Amount;

        /// <summary>Gets or sets the paid amount.</summary>
        [JsonProperty("payment_amount")]
        public string? PaymentAmount { get; set; } = webhook.PaymentAmount;

        /// <summary>Gets or sets the paid amount in USD.</summary>
        [JsonProperty("payment_amount_usd")]
        public string? PaymentAmountUsd { get; set; } = webhook.PaymentAmountUsd;

        /// <summary>Gets or sets the merchant amount.</summary>
        [JsonProperty("merchant_amount")]
        public string? MerchantAmount { get; set; } = webhook.MerchantAmount;

        /// <summary>Gets or sets the commission.</summary>
        [JsonProperty("commission")]
        public string? Commission { get; set; } = webhook.Commission;

        /// <summary>Gets or sets whether the webhook is final.</summary>
        [JsonProperty("is_final")]
        public bool? IsFinal { get; set; } = webhook.IsFinal;

        /// <summary>Gets or sets the payment status.</summary>
        [JsonProperty("status")]
        public string? Status { get; set; } = webhook.Status;

        /// <summary>Gets or sets the sender address.</summary>
        [JsonProperty("from")]
        public string? From { get; set; } = webhook.From;

        /// <summary>Gets or sets the wallet address UUID.</summary>
        [JsonProperty("wallet_address_uuid")]
        public string? WalletAddressUuid { get; set; } = webhook.WalletAddressUuid;

        /// <summary>Gets or sets the network code.</summary>
        [JsonProperty("network")]
        public string? Network { get; set; } = webhook.Network;

        /// <summary>Gets or sets the invoice currency code.</summary>
        [JsonProperty("currency")]
        public string? Currency { get; set; } = webhook.Currency;

        /// <summary>Gets or sets the payer currency code.</summary>
        [JsonProperty("payer_currency")]
        public string? PayerCurrency { get; set; } = webhook.PayerCurrency;

        /// <summary>Gets or sets merchant-defined additional data.</summary>
        [JsonProperty("additional_data")]
        public string? AdditionalData { get; set; } = webhook.AdditionalData;

        /// <summary>Gets or sets conversion information.</summary>
        [JsonProperty("convert")]
        public ConversionInfo? Convert { get; set; } = webhook.Convert;

        /// <summary>Gets or sets the transaction identifier.</summary>
        [JsonProperty("txid")]
        public string? Txid { get; set; } = webhook.Txid;
    }
}
