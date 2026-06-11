using System.Globalization;
using Newtonsoft.Json;

namespace Heleket.Payments.Requests;

/// <summary>
/// Request body for creating a Heleket payment invoice.
/// </summary>
public sealed class CreatePaymentRequest
{
    /// <summary>Gets the invoice amount as a string wire value.</summary>
    [JsonProperty("amount")]
    public required string Amount { get; init; }

    /// <summary>Gets the fiat or cryptocurrency invoice currency code.</summary>
    [JsonProperty("currency")]
    public required string Currency { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonProperty("order_id")]
    public required string OrderId { get; init; }

    /// <summary>Gets the optional blockchain network code.</summary>
    [JsonProperty("network")]
    public string? Network { get; init; }

    /// <summary>Gets the optional URL returned to before payment completion.</summary>
    [JsonProperty("url_return")]
    public string? UrlReturn { get; init; }

    /// <summary>Gets the optional URL returned to after successful payment.</summary>
    [JsonProperty("url_success")]
    public string? UrlSuccess { get; init; }

    /// <summary>Gets the optional webhook callback URL.</summary>
    [JsonProperty("url_callback")]
    public string? UrlCallback { get; init; }

    /// <summary>Gets whether multiple partial payments are allowed.</summary>
    [JsonProperty("is_payment_multiple")]
    public bool? IsPaymentMultiple { get; init; }

    /// <summary>Gets the invoice lifetime in seconds.</summary>
    [JsonProperty("lifetime")]
    public int? LifetimeSeconds { get; init; }

    /// <summary>Gets the target cryptocurrency for conversion.</summary>
    [JsonProperty("to_currency")]
    public string? ToCurrency { get; init; }

    /// <summary>Gets the fee subtraction percentage.</summary>
    [JsonProperty("subtract")]
    public int? Subtract { get; init; }

    /// <summary>Gets the accepted payment accuracy percentage.</summary>
    [JsonProperty("accuracy_payment_percent")]
    public decimal? AccuracyPaymentPercent { get; init; }

    /// <summary>Gets merchant-defined additional data.</summary>
    [JsonProperty("additional_data")]
    public string? AdditionalData { get; init; }

    /// <summary>Gets the allowed payment currencies.</summary>
    [JsonProperty("currencies")]
    public IReadOnlyList<PaymentCurrency>? Currencies { get; init; }

    /// <summary>Gets the excluded payment currencies.</summary>
    [JsonProperty("except_currencies")]
    public IReadOnlyList<PaymentCurrency>? ExceptCurrencies { get; init; }

    /// <summary>Gets the exchange rate source.</summary>
    [JsonProperty("course_source")]
    public string? CourseSource { get; init; }

    /// <summary>Gets the referral code.</summary>
    [JsonProperty("from_referral_code")]
    public string? FromReferralCode { get; init; }

    /// <summary>Gets the payment method discount or surcharge percentage.</summary>
    [JsonProperty("discount_percent")]
    public int? DiscountPercent { get; init; }

    /// <summary>Gets whether to refresh an expired invoice.</summary>
    [JsonProperty("is_refresh")]
    public bool? IsRefresh { get; init; }

    /// <summary>Gets the payer email address.</summary>
    [JsonProperty("payer_email")]
    public string? PayerEmail { get; init; }

    /// <summary>
    /// Creates a request from a decimal amount using invariant-culture formatting.
    /// </summary>
    /// <param name="amount">The amount to format.</param>
    /// <param name="currency">The invoice currency code.</param>
    /// <param name="orderId">The merchant order identifier.</param>
    /// <returns>A new payment creation request.</returns>
    public static CreatePaymentRequest FromDecimal(decimal amount, string currency, string orderId)
    {
        return new CreatePaymentRequest
        {
            Amount = amount.ToString("0.#############################", CultureInfo.InvariantCulture),
            Currency = currency,
            OrderId = orderId
        };
    }
}

/// <summary>
/// Currency and optional network selector used by Heleket payment creation filters.
/// </summary>
public sealed class PaymentCurrency
{
    /// <summary>Gets the currency code.</summary>
    [JsonProperty("currency")]
    public required string Currency { get; init; }

    /// <summary>Gets the optional blockchain network code.</summary>
    [JsonProperty("network")]
    public string? Network { get; init; }
}
