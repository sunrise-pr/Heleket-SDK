using Newtonsoft.Json;

namespace Heleket.Payments;

/// <summary>
/// Represents a Heleket payment status as an extensible string value.
/// </summary>
[JsonConverter(typeof(PaymentStatusJsonConverter))]
public readonly record struct PaymentStatus(string Value)
{
    /// <summary>Payment is waiting for confirmation checks.</summary>
    public static PaymentStatus ConfirmCheck { get; } = new("confirm_check");

    /// <summary>Payment was completed for the expected amount.</summary>
    public static PaymentStatus Paid { get; } = new("paid");

    /// <summary>Payment was completed for more than the expected amount.</summary>
    public static PaymentStatus PaidOver { get; } = new("paid_over");

    /// <summary>Payment failed.</summary>
    public static PaymentStatus Fail { get; } = new("fail");

    /// <summary>Payment was received for the wrong amount.</summary>
    public static PaymentStatus WrongAmount { get; } = new("wrong_amount");

    /// <summary>Payment was canceled.</summary>
    public static PaymentStatus Cancel { get; } = new("cancel");

    /// <summary>Payment failed because of a system error.</summary>
    public static PaymentStatus SystemFail { get; } = new("system_fail");

    /// <summary>Payment refund is in progress.</summary>
    public static PaymentStatus RefundProcess { get; } = new("refund_process");

    /// <summary>Payment refund failed.</summary>
    public static PaymentStatus RefundFail { get; } = new("refund_fail");

    /// <summary>Payment refund was completed.</summary>
    public static PaymentStatus RefundPaid { get; } = new("refund_paid");

    /// <summary>
    /// Determines whether the status is a successful final payment status.
    /// </summary>
    /// <param name="status">The payment status.</param>
    /// <param name="isFinal">Whether Heleket marked the payment as final.</param>
    /// <returns><see langword="true"/> for final `paid` or `paid_over` statuses.</returns>
    public static bool IsSuccessfulFinal(PaymentStatus status, bool isFinal)
    {
        return isFinal && (status == Paid || status == PaidOver);
    }

    /// <summary>
    /// Determines whether the status is a failure status.
    /// </summary>
    /// <param name="status">The payment status.</param>
    /// <returns><see langword="true"/> for known failure statuses.</returns>
    public static bool IsFailure(PaymentStatus status)
    {
        return status == Fail || status == WrongAmount || status == Cancel || status == SystemFail;
    }

    /// <summary>
    /// Determines whether the status is a refund-related status.
    /// </summary>
    /// <param name="status">The payment status.</param>
    /// <returns><see langword="true"/> for known refund statuses.</returns>
    public static bool IsRefund(PaymentStatus status)
    {
        return status == RefundProcess || status == RefundFail || status == RefundPaid;
    }

    /// <summary>
    /// Determines whether this status is a successful final payment status.
    /// </summary>
    /// <param name="isFinal">Whether Heleket marked the payment as final.</param>
    /// <returns><see langword="true"/> for final `paid` or `paid_over` statuses.</returns>
    public bool IsSuccessfulFinal(bool isFinal) => IsSuccessfulFinal(this, isFinal);

    /// <summary>
    /// Determines whether this status is a failure status.
    /// </summary>
    /// <returns><see langword="true"/> for known failure statuses.</returns>
    public bool IsFailure() => IsFailure(this);

    /// <summary>
    /// Determines whether this status is refund-related.
    /// </summary>
    /// <returns><see langword="true"/> for known refund statuses.</returns>
    public bool IsRefund() => IsRefund(this);

    /// <summary>
    /// Converts the status to its raw Heleket string value.
    /// </summary>
    /// <returns>The raw status value.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Creates a status from a raw string value.
    /// </summary>
    /// <param name="value">The raw status value.</param>
    public static implicit operator PaymentStatus(string value) => new(value);

    /// <summary>
    /// Converts a status to its raw string value.
    /// </summary>
    /// <param name="status">The status value.</param>
    public static implicit operator string(PaymentStatus status) => status.Value;
}

internal sealed class PaymentStatusJsonConverter : JsonConverter<PaymentStatus>
{
    public override void WriteJson(JsonWriter writer, PaymentStatus value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Value);
    }

    public override PaymentStatus ReadJson(
        JsonReader reader,
        Type objectType,
        PaymentStatus existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        return reader.Value is null ? default : new PaymentStatus(reader.Value.ToString()!);
    }
}
