using Heleket.Common;
using Heleket.Payments.Requests;
using Heleket.Payments.Responses;

namespace Heleket.Abstractions;

/// <summary>
/// Provides Heleket payment operations for the MVP payment API surface.
/// </summary>
public interface IHeleketPaymentsClient
{
    /// <summary>
    /// Creates or retrieves a Heleket payment invoice.
    /// </summary>
    /// <param name="request">The payment creation request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The Heleket response envelope containing the payment.</returns>
    Task<HeleketResponse<HeleketPayment>> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets payment information by UUID or order ID.
    /// </summary>
    /// <param name="request">The payment lookup request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The Heleket response envelope containing the payment.</returns>
    Task<HeleketResponse<HeleketPayment>> GetInfoAsync(GetPaymentInfoRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Requests a payment refund.
    /// </summary>
    /// <param name="request">The refund request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The Heleket response envelope containing refund details.</returns>
    Task<HeleketResponse<RefundPaymentResponse>> RefundAsync(RefundPaymentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets payment services supported by Heleket.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The Heleket response envelope containing payment services.</returns>
    Task<HeleketResponse<IReadOnlyList<PaymentService>>> GetServicesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets payment history with optional cursor pagination.
    /// </summary>
    /// <param name="request">The payment history filter.</param>
    /// <param name="cursor">The optional pagination cursor.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The Heleket response envelope containing a payment page.</returns>
    Task<HeleketResponse<HeleketPage<HeleketPayment>>> GetHistoryAsync(
        PaymentHistoryRequest request,
        string? cursor = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Requests that Heleket resend a payment webhook.
    /// </summary>
    /// <param name="request">The payment webhook resend request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The Heleket response envelope containing resend details.</returns>
    Task<HeleketResponse<ResendPaymentWebhookResult>> ResendWebhookAsync(
        ResendPaymentWebhookRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Requests that Heleket send a payment test webhook.
    /// </summary>
    /// <param name="request">The test webhook request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The Heleket response envelope containing test webhook details.</returns>
    Task<HeleketResponse<TestPaymentWebhookResult>> SendTestWebhookAsync(
        TestPaymentWebhookRequest request,
        CancellationToken cancellationToken = default);
}
