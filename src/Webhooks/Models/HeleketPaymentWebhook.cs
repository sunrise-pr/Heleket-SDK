using Heleket.Payments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Heleket.Webhooks.Models;

/// <summary>
/// Represents a typed Heleket payment webhook payload after raw-body signature validation.
/// </summary>
public sealed class HeleketPaymentWebhook
{
    /// <summary>Gets the webhook type.</summary>
    [JsonProperty("type")]
    public string? Type { get; init; }

    /// <summary>Gets the Heleket payment UUID.</summary>
    [JsonProperty("uuid")]
    public string? Uuid { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonProperty("order_id")]
    public string? OrderId { get; init; }

    /// <summary>Gets the invoice amount.</summary>
    [JsonProperty("amount")]
    public string? Amount { get; init; }

    /// <summary>Gets the paid amount.</summary>
    [JsonProperty("payment_amount")]
    public string? PaymentAmount { get; init; }

    /// <summary>Gets the paid amount in USD.</summary>
    [JsonProperty("payment_amount_usd")]
    public string? PaymentAmountUsd { get; init; }

    /// <summary>Gets the merchant amount.</summary>
    [JsonProperty("merchant_amount")]
    public string? MerchantAmount { get; init; }

    /// <summary>Gets the commission.</summary>
    [JsonProperty("commission")]
    public string? Commission { get; init; }

    /// <summary>Gets whether the webhook is final.</summary>
    [JsonProperty("is_final")]
    public bool? IsFinal { get; init; }

    /// <summary>Gets the payment status.</summary>
    [JsonProperty("status")]
    public PaymentStatus? Status { get; init; }

    /// <summary>Gets the sender address.</summary>
    [JsonProperty("from")]
    public string? From { get; init; }

    /// <summary>Gets the wallet address UUID.</summary>
    [JsonProperty("wallet_address_uuid")]
    public string? WalletAddressUuid { get; init; }

    /// <summary>Gets the blockchain network code.</summary>
    [JsonProperty("network")]
    public string? Network { get; init; }

    /// <summary>Gets the invoice currency code.</summary>
    [JsonProperty("currency")]
    public string? Currency { get; init; }

    /// <summary>Gets the payer currency code.</summary>
    [JsonProperty("payer_currency")]
    public string? PayerCurrency { get; init; }

    /// <summary>Gets merchant-defined additional data.</summary>
    [JsonProperty("additional_data")]
    public string? AdditionalData { get; init; }

    /// <summary>Gets conversion details when returned by Heleket.</summary>
    [JsonProperty("convert")]
    public JToken? Convert { get; init; }

    /// <summary>Gets the transaction identifier.</summary>
    [JsonProperty("txid")]
    public string? Txid { get; init; }

    /// <summary>Gets the received webhook signature.</summary>
    [JsonProperty("sign")]
    public string? Sign { get; init; }

    /// <summary>Gets additional JSON fields returned by Heleket.</summary>
    [JsonExtensionData]
    public IDictionary<string, JToken>? ExtensionData { get; init; }
}
