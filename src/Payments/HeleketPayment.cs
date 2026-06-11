using System.Text.Json;
using System.Text.Json.Serialization;

namespace Heleket.Payments;

/// <summary>
/// Payment payload returned by Heleket payment endpoints.
/// </summary>
public sealed class HeleketPayment
{
    /// <summary>Gets the Heleket payment UUID.</summary>
    [JsonPropertyName("uuid")]
    public string? Uuid { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonPropertyName("order_id")]
    public string? OrderId { get; init; }

    /// <summary>Gets the invoice amount as a string wire value.</summary>
    [JsonPropertyName("amount")]
    public string? Amount { get; init; }

    /// <summary>Gets the paid cryptocurrency amount as a string wire value.</summary>
    [JsonPropertyName("payment_amount")]
    public string? PaymentAmount { get; init; }

    /// <summary>Gets the payer amount as a string wire value.</summary>
    [JsonPropertyName("payer_amount")]
    public string? PayerAmount { get; init; }

    /// <summary>Gets the applied discount percentage.</summary>
    [JsonPropertyName("discount_percent")]
    public int? DiscountPercent { get; init; }

    /// <summary>Gets the applied discount amount as a string wire value.</summary>
    [JsonPropertyName("discount")]
    public string? Discount { get; init; }

    /// <summary>Gets the payer currency code.</summary>
    [JsonPropertyName("payer_currency")]
    public string? PayerCurrency { get; init; }

    /// <summary>Gets the invoice currency code.</summary>
    [JsonPropertyName("currency")]
    public string? Currency { get; init; }

    /// <summary>Gets the merchant amount as a string wire value.</summary>
    [JsonPropertyName("merchant_amount")]
    public string? MerchantAmount { get; init; }

    /// <summary>Gets the blockchain network code.</summary>
    [JsonPropertyName("network")]
    public string? Network { get; init; }

    /// <summary>Gets the payment address.</summary>
    [JsonPropertyName("address")]
    public string? Address { get; init; }

    /// <summary>Gets the sender address.</summary>
    [JsonPropertyName("from")]
    public string? From { get; init; }

    /// <summary>Gets the blockchain transaction identifier.</summary>
    [JsonPropertyName("txid")]
    public string? Txid { get; init; }

    /// <summary>Gets the payment status field.</summary>
    [JsonPropertyName("payment_status")]
    public string? PaymentStatus { get; init; }

    /// <summary>Gets the status field.</summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>Gets the hosted invoice URL.</summary>
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    /// <summary>Gets the invoice expiration timestamp.</summary>
    [JsonPropertyName("expired_at")]
    public long? ExpiredAt { get; init; }

    /// <summary>Gets whether the payment is final.</summary>
    [JsonPropertyName("is_final")]
    public bool? IsFinal { get; init; }

    /// <summary>Gets merchant-defined additional data.</summary>
    [JsonPropertyName("additional_data")]
    public string? AdditionalData { get; init; }

    /// <summary>Gets the creation timestamp.</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>Gets the update timestamp.</summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }

    /// <summary>Gets the payment address QR code payload.</summary>
    [JsonPropertyName("address_qr_code")]
    public string? AddressQrCode { get; init; }

    /// <summary>Gets the payment amount in USD as a string wire value.</summary>
    [JsonPropertyName("payment_amount_usd")]
    public string? PaymentAmountUsd { get; init; }

    /// <summary>Gets the commission as a string wire value.</summary>
    [JsonPropertyName("commission")]
    public string? Commission { get; init; }

    /// <summary>Gets conversion details when returned by Heleket.</summary>
    [JsonPropertyName("convert")]
    public JsonElement? Convert { get; init; }

    /// <summary>Gets additional JSON fields returned by Heleket.</summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; init; }
}
