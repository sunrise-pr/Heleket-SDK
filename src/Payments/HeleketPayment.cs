using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heleket.Payments;

/// <summary>
/// Payment payload returned by Heleket payment endpoints.
/// </summary>
public sealed class HeleketPayment
{
    /// <summary>Gets the Heleket payment UUID.</summary>
    [JsonProperty("uuid")]
    public string? Uuid { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonProperty("order_id")]
    public string? OrderId { get; init; }

    /// <summary>Gets the invoice amount as a string wire value.</summary>
    [JsonProperty("amount")]
    public string? Amount { get; init; }

    /// <summary>Gets the paid cryptocurrency amount as a string wire value.</summary>
    [JsonProperty("payment_amount")]
    public string? PaymentAmount { get; init; }

    /// <summary>Gets the payer amount as a string wire value.</summary>
    [JsonProperty("payer_amount")]
    public string? PayerAmount { get; init; }

    /// <summary>Gets the payer amount exchange rate as a string wire value.</summary>
    [JsonProperty("payer_amount_exchange_rate")]
    public string? PayerAmountExchangeRate { get; init; }

    /// <summary>Gets the applied discount percentage.</summary>
    [JsonProperty("discount_percent")]
    public int? DiscountPercent { get; init; }

    /// <summary>Gets the applied discount amount as a string wire value.</summary>
    [JsonProperty("discount")]
    public string? Discount { get; init; }

    /// <summary>Gets the payer currency code.</summary>
    [JsonProperty("payer_currency")]
    public string? PayerCurrency { get; init; }

    /// <summary>Gets the invoice currency code.</summary>
    [JsonProperty("currency")]
    public string? Currency { get; init; }

    /// <summary>Gets payment comments when returned by Heleket.</summary>
    [JsonProperty("comments")]
    public string? Comments { get; init; }

    /// <summary>Gets the merchant amount as a string wire value.</summary>
    [JsonProperty("merchant_amount")]
    public string? MerchantAmount { get; init; }

    /// <summary>Gets the blockchain network code.</summary>
    [JsonProperty("network")]
    public string? Network { get; init; }

    /// <summary>Gets the payment address.</summary>
    [JsonProperty("address")]
    public string? Address { get; init; }

    /// <summary>Gets the sender address.</summary>
    [JsonProperty("from")]
    public string? From { get; init; }

    /// <summary>Gets the blockchain transaction identifier.</summary>
    [JsonProperty("txid")]
    public string? Txid { get; init; }

    /// <summary>Gets the payment status field.</summary>
    [JsonProperty("payment_status")]
    public string? PaymentStatus { get; init; }

    /// <summary>Gets the status field.</summary>
    [JsonProperty("status")]
    public string? Status { get; init; }

    /// <summary>Gets the hosted invoice URL.</summary>
    [JsonProperty("url")]
    public string? Url { get; init; }

    /// <summary>Gets the invoice expiration timestamp.</summary>
    [JsonProperty("expired_at")]
    public long? ExpiredAt { get; init; }

    /// <summary>Gets whether the payment is final.</summary>
    [JsonProperty("is_final")]
    public bool? IsFinal { get; init; }

    /// <summary>Gets merchant-defined additional data.</summary>
    [JsonProperty("additional_data")]
    public string? AdditionalData { get; init; }

    /// <summary>Gets the creation timestamp.</summary>
    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>Gets the update timestamp.</summary>
    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }

    /// <summary>Gets the payment address QR code payload.</summary>
    [JsonProperty("address_qr_code")]
    public string? AddressQrCode { get; init; }

    /// <summary>Gets the payment amount in USD as a string wire value.</summary>
    [JsonProperty("payment_amount_usd")]
    public string? PaymentAmountUsd { get; init; }

    /// <summary>Gets the commission as a string wire value.</summary>
    [JsonProperty("commission")]
    public string? Commission { get; init; }

    /// <summary>Gets conversion details when returned by Heleket.</summary>
    [JsonProperty("convert")]
    public JToken? Convert { get; init; }

    /// <summary>Gets additional JSON fields returned by Heleket.</summary>
    [JsonExtensionData]
    public IDictionary<string, JToken>? ExtensionData { get; init; }
}
