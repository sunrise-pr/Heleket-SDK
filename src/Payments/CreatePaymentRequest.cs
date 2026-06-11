using System.Globalization;
using System.Text.Json.Serialization;

namespace Heleket.Payments;

/// <summary>
/// Request body for creating a Heleket payment invoice.
/// </summary>
public sealed class CreatePaymentRequest
{
    /// <summary>Gets the invoice amount as a string wire value.</summary>
    [JsonPropertyName("amount")]
    public required string Amount { get; init; }

    /// <summary>Gets the fiat or cryptocurrency invoice currency code.</summary>
    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    /// <summary>Gets the merchant order identifier.</summary>
    [JsonPropertyName("order_id")]
    public required string OrderId { get; init; }

    /// <summary>Gets the optional blockchain network code.</summary>
    [JsonPropertyName("network")]
    public string? Network { get; init; }

    /// <summary>Gets the optional URL returned to before payment completion.</summary>
    [JsonPropertyName("url_return")]
    public string? UrlReturn { get; init; }

    /// <summary>Gets the optional URL returned to after successful payment.</summary>
    [JsonPropertyName("url_success")]
    public string? UrlSuccess { get; init; }

    /// <summary>Gets the optional webhook callback URL.</summary>
    [JsonPropertyName("url_callback")]
    public string? UrlCallback { get; init; }

    /// <summary>Gets whether multiple partial payments are allowed.</summary>
    [JsonPropertyName("is_payment_multiple")]
    public bool? IsPaymentMultiple { get; init; }

    /// <summary>Gets the invoice lifetime in seconds.</summary>
    [JsonPropertyName("lifetime")]
    public int? LifetimeSeconds { get; init; }

    /// <summary>Gets the target cryptocurrency for conversion.</summary>
    [JsonPropertyName("to_currency")]
    public string? ToCurrency { get; init; }

    /// <summary>Gets the fee subtraction percentage.</summary>
    [JsonPropertyName("subtract")]
    public int? Subtract { get; init; }

    /// <summary>Gets the accepted payment accuracy percentage.</summary>
    [JsonPropertyName("accuracy_payment_percent")]
    public decimal? AccuracyPaymentPercent { get; init; }

    /// <summary>Gets merchant-defined additional data.</summary>
    [JsonPropertyName("additional_data")]
    public string? AdditionalData { get; init; }

    /// <summary>Gets the exchange rate source.</summary>
    [JsonPropertyName("course_source")]
    public string? CourseSource { get; init; }

    /// <summary>Gets the referral code.</summary>
    [JsonPropertyName("from_referral_code")]
    public string? FromReferralCode { get; init; }

    /// <summary>Gets the payment method discount or surcharge percentage.</summary>
    [JsonPropertyName("discount_percent")]
    public int? DiscountPercent { get; init; }

    /// <summary>Gets whether to refresh an expired invoice.</summary>
    [JsonPropertyName("is_refresh")]
    public bool? IsRefresh { get; init; }

    /// <summary>Gets the payer email address.</summary>
    [JsonPropertyName("payer_email")]
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
