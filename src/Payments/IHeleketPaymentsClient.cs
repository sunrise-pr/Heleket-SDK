namespace Heleket.Payments;

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
}
